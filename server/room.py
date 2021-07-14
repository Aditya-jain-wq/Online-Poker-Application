from dataclasses import dataclass
from typing import Any, List

from commands import Command

MAX_PLAYERS_IN_GAME = 8


class Player:
    conn: Any
    username: str
    money: int
    cards: List[str] = []

    def send(room: "Room"):
        pass


@dataclass
class Room:
    default_amt: int
    dealer_cards: List[str] = []
    players: List[Player] = []
    pot: int = 0
    started: bool = False

    def add_player(self, conn: Any, username: str):
        player = Player(conn, username, self.default_amt)
        self.players.append(player)

    def update(self, cmd: Command):
        pass
