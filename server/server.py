from socketserver import TCPServer, BaseRequestHandler
import json
from typing import Dict

from commands import *
from room import Room, MAX_PLAYERS_IN_GAME as MPIG


class PokerHandler(BaseRequestHandler):
    def __init__(self, rooms: Dict[str, Room]):
        self.rooms = rooms

    def handle(self):
        self.data = self.request.recv(16192).strip()
        payload = json.loads(self.data)
        if payload["kind"] == "JOIN":
            cmd = JoinCmd(payload["payload"])
            assert (
                len(self.rooms[cmd.room].players) < MPIG
                and not self.rooms[cmd.room].started
            )
            self.rooms[cmd.room].players.append((self.client_address, cmd.username))
        elif payload["kind"] == "CREATE":
            cmd = CreateCmd(payload["payload"])
            pass
        else:
            cmd = {
                "RAISE": RaiseCmd,
                "FOLD": FoldCmd,
            }[payload["kind"]]
            cmd = cmd(**payload["cmd"])
            room = cmd.room
            self.rooms[room] = self.rooms[room].update(cmd)
            # self.request.sendall(self.rooms[room])
        # self.request.sendall(self.data.upper())


if __name__ == "__main__":
    PORT = 12345
    rooms = {}

    with TCPServer(("", PORT), PokerHandler(rooms)) as server:
        server.serve_forever()
