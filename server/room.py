from dataclasses import dataclass, field
from typing import Any, List

from commands import Command

MAX_PLAYERS_IN_GAME = 8


@dataclass
class Player:
    conn: Any
    username: str
    money: int
    put_in_pot: int = 0
    is_live: bool = True
    cards: List[str] = field(default_factory=list)

    def send(room: "Room"):
        pass


class CardDeck:
    DECK_LETTER = [
        "H",
        "D",
        "C",
        "S",
    ]
    DECK_VALUE = ["A", *(str(i) for i in range(2, 11)), "J", "Q", "K"]

    def __init__(self) -> None:
        self.cards = [False for _ in range(52)]

    @staticmethod
    def idx_to_card(card: int) -> str:
        "DECK_NUMBER"
        deck = CardDeck.DECK_LETTER[card // 13]
        val = CardDeck.DECK_VALUE[card % 13]
        return f"{deck}{val}"

    def get_new_card(self) -> str:
        import random

        card = random.choice([i for i, j in enumerate(self.cards) if not j])
        self.cards[card] = True
        return self.idx_to_card(card)


@dataclass
class Room:
    id_: str
    player_this_turn: int = 0
    default_amt: int = 100
    dealer_cards: List[str] = field(default_factory=list)
    players: List[Player] = field(default_factory=list)
    pot: int = 0
    started: bool = False
    card_deck: CardDeck = field(default_factory=CardDeck)

    def add_player(self, conn: Any, username: str):
        player = Player(conn, username, self.default_amt)
        self.players.append(player)

    def update(self, cmd: Command):
        cmd.update(self)

    def play_dealer(self):
        pass
