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
        kind = payload["kind"]
        username = payload["username"]
        room_id = payload["room"]
        payload = payload["payload"]
        room: Room
        if kind == "JOIN":
            assert self.players[conn] == ""
            assert room_id in self.rooms
            room = self.rooms[room_id]
            room.add_player(conn, username)
            self.players[conn] = room_id
        elif kind == "CREATE":
            assert self.players[conn] == ""
            assert room_id == ""
            room_id = get_random_room(self.rooms.keys())
            room = Room(room_id)
            room.add_player(conn, username)
            self.rooms[room_id] = room
            self.players[conn] = room_id
        else:
            cmd = {
                "START": StartCmd,
                "BET": BetCmd,
                "FOLD": FoldCmd,
            }[kind]
            cmd = cmd(**payload)
            self.rooms[room_id].update(cmd, username)
            room = self.rooms[room_id]
        for pl in room.players:
            pl.send(room)

    def handle(self, conn):
        try:
            self._handle(conn)
        except Exception as e:
            ## TODO: return an error message based on the exception
            conn.send(e)

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
