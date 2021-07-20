import socket
import selectors
import json
from typing import Dict

from commands import *
from room import Room, MAX_PLAYERS_IN_GAME as MPIG


def get_random_room(rooms) -> str:
    """Return a room id that is not present in rooms"""
    pass


class PokerServer:
    def __init__(self, host: str, port: int):
        self.listener = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.listener.bind((host, port))
        self.listener.setblocking(False)
        self.listener.listen(10)
        self.sel = selectors.DefaultSelector()
        self.sel.register(self.listener, selectors.EVENT_READ, self.accept)
        self.players = {}
        self.rooms = {}

    def accept(self, _):
        conn, _client = self.listener.accept()
        self.players[conn] = ""
        self.sel.register(conn, selectors.EVENT_READ, self.handle)

    def _handle(self, conn):
        data = conn.recv(16192).strip()
        payload = json.loads(data)
        username = payload["username"]
        kind = payload["kind"]
        payload = payload["payload"]
        room: Room
        if kind == "JOIN":
            cmd = JoinCmd(**payload)
            assert self.players[conn] == ""
            assert cmd.room in self.rooms
            room = self.rooms[cmd.room]
            assert len(room.players) < MPIG
            assert not room.started
            room.add_player(conn, username)
            self.players[conn] = cmd.room
        elif payload["kind"] == "CREATE":
            cmd = CreateCmd(**payload)
            assert self.players[conn] == ""
            new_room = get_random_room(self.rooms.keys())
            new_state = Room(cmd.default_amt)
            new_state.add_player(conn, username)
            self.rooms[new_room] = new_state
            self.players[conn] = new_room
            room = new_state
        else:
            cmd = {
                "START": StartCmd,
                "RAISE": RaiseCmd,
                "FOLD": FoldCmd,
            }[kind]
            cmd = cmd(**payload)
            room = cmd.room
            self.rooms[room].update(cmd)
            room = self.rooms[cmd.room]
        for pl in room.players:
            pl.send(room)

    def handle(self, conn):
        try:
            self._handle(conn)
        except Exception as e:
            # return an error message based on the exception
            pass

    def serve(self):
        while True:
            events = self.sel.select()
            for key, _ in events:
                callback = key.data
                callback(key.fileobj)


if __name__ == "__main__":
    PORT = 12345

    server = PokerServer("", PORT)
    print("Starting server...")
    server.serve()
