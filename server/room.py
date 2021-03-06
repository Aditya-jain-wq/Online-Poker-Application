from dataclasses import dataclass, field
import json
from typing import Any, List, Optional

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

    def send(self, room: "Room"):
        from itertools import chain, repeat, islice

        data = {
            "room_id": room.id_,
            "pot_amt": room.pot,
            "dealer_cards": [*islice(chain(room.dealer_cards, repeat("XX")), 5)],
            "winner": room.winner,
            "players": [
                {
                    "username": pl.username,
                    "pot_contrib": pl.put_in_pot,
                    "total_money": pl.money,
                    "cards": pl.cards if pl.username == self.username else ["XX", "XX"],
                    "is_live": pl.is_live,
                    "is_turn_now": False,
                }
                for pl in room.players
            ],
        }
        data["players"] = list(islice(chain(data["players"], repeat(None)), 8))
        if room.is_started:
            data["players"][room.player_this_turn]["is_turn_now"] = True
        json_data = json.dumps(data)
        self.conn.send(json_data.encode())


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
    winner: str = ""
    player_this_turn: int = 0
    default_amt: int = 100
    dealer_cards: List[str] = field(default_factory=list)
    players: List[Player] = field(default_factory=list)
    pot: int = 0
    is_started: bool = False
    card_deck: CardDeck = field(default_factory=CardDeck)
    bets_this_round: int = 0

    @property
    def live_players(self):
        return len([pl for pl in self.players if pl.is_live])

    def add_player(self, conn: Any, username: str):
        assert not self.is_started
        assert len(self.players) < MAX_PLAYERS_IN_GAME
        assert len([pl for pl in self.players if pl.username == username]) == 0
        player = Player(conn, username, self.default_amt)
        self.players.append(player)

    def get_player(self, username: str) -> Player:
        for pl in self.players:
            if pl.username == username:
                return pl

    def next_player(self):
        self.player_this_turn = (self.player_this_turn + 1) % len(self.players)
        while not self.players[self.player_this_turn].is_live:
            self.player_this_turn = (self.player_this_turn + 1) % len(self.players)

    def play_dealer(self):
        self.bets_this_round = 0
        if len(self.dealer_cards) == 0:
            self.dealer_cards = [
                self.card_deck.get_new_card(),
                self.card_deck.get_new_card(),
                self.card_deck.get_new_card(),
            ]
        else:
            self.dealer_cards.append(self.card_deck.get_new_card())
        self.player_this_turn = 0
        while not self.players[self.player_this_turn].is_live:
            self.player_this_turn = (self.player_this_turn + 1) % len(self.players)

    def update(self, cmd: Command, username: str):
        assert self.players[self.player_this_turn].username == username
        cmd.update(self, username)
        self.next_player()
        if self.live_players == 1:
            # second last player folded
            live_player = self.players[self.player_this_turn]
            self.winner = live_player.username
        elif self.bets_this_round >= self.live_players and all(
            pl.put_in_pot == self.players[self.player_this_turn].put_in_pot
            for pl in self.players
            if pl.is_live
        ):
            # betting round has ended
            if len(self.dealer_cards) < 5:
                self.play_dealer()
            else:
                # game ended
                ## TODO: calculate scores and find winner
                self.winner = self.players[self.player_this_turn].username
