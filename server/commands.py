from dataclasses import dataclass
from typing import Protocol, TYPE_CHECKING

if TYPE_CHECKING:
    from server.room import Player, Room


class Command(Protocol):
    def update(self, room: "Room", user: str):
        ...


@dataclass
class BetCmd:
    amt: int  # amount that player is adding to the pot this turn
    user: str

    def update(self, room: "Room"):
        player: "Player"
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
    def update(self, room: "Room", user: str):
        player = [pl for pl in room.players if pl.username == user][0]
        player.is_live = False
        if room.live_players == 1:
            live_player = [pl for pl in room.players if pl.is_live][0]
            room.winner = live_player.username


@dataclass
class StartCmd:
    def update(self, room: "Room", user: str):
        assert not room.is_started
        assert len(room.players) >= 2
        assert room.players[0].username == user
        room.is_started = True
        for pl in room.players:
            pl.cards = [room.card_deck.get_new_card(), room.card_deck.get_new_card()]
