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

    def update(self, room: "Room", user: str):
        player = room.get_player(user)
        assert player.money >= self.amt + player.put_in_pot
        room.pot += self.amt
        player.put_in_pot += self.amt
        room.bets_this_round += 1


@dataclass
class FoldCmd:
    def update(self, room: "Room", user: str):
        player = room.get_player(user)
        player.is_live = False


@dataclass
class StartCmd:
    def update(self, room: "Room", user: str):
        assert not room.is_started
        assert len(room.players) >= 2
        assert room.players[0].username == user
        room.is_started = True
        for pl in room.players:
            pl.cards = [room.card_deck.get_new_card(), room.card_deck.get_new_card()]
