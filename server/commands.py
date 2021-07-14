from dataclasses import dataclass
from typing import Protocol


class Command(Protocol):
    pass


@dataclass
class JoinCmd:
    room: str


@dataclass
class CreateCmd:
    default_amt: int


@dataclass
class RaiseCmd:
    pass


@dataclass
class FoldCmd:
    pass


@dataclass
class StartCmd:
    pass
