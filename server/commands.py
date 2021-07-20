from dataclasses import dataclass
from server.room import Player, Room
from typing import Protocol


class Command(Protocol):
    def update(self, room: Room):
        ...


@dataclass
class JoinCmd:
    room: str


@dataclass
class CreateCmd:
    default_amt: int


@dataclass
class RaiseCmd:
    amt: int  # amount that player is adding to the pot this turn
    user: str

    def update(self, room: Room):
        player: Player
        for pl in room.players:
            if pl.username == self.user:
                player = pl
        assert player.money >= self.amt + player.put_in_pot
        player.put_in_pot += self.amt
        if len(
            pl for pl in room.players if pl.put_in_pot == room.players[0].put_in_pot
        ) == len(room.players):
            room.play_dealer()


@dataclass
class FoldCmd:
    user: str

    def update(self, room: Room):
        player = [pl for pl in room.players if pl.username == self.user][0]
        player.is_live = False


@dataclass
class StartCmd:
    def update(self, room: Room):
        assert not room.started
        room.started = True
