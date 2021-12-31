# crypto-game-proto
A true crypto game prototype.

The idea is to make an truly decentralized game where the entire state of the game is deterministically
derived from a blockchain. This is in contrast to the current "crypto game" system where things are
in actually centralized becaues the companies running these things have private databases and can
make your NFTs unusable within their game, thereby ruining their value.

The idea behind this game is to make a multiplayer simulation/idle game where the "ticks" of the
game server are the blocks of a blockchain. Think trimps or something like that or a "lockstep RTS".

This is a bit like the start of a Civ game with players and lands that generate resources each tick.

There are two projects:
	
	CryptoGame: a Unity client that scans a certain blob/s3 storage account for certain files that will download all
		commands (OP_RETURN) sent to that blockchain.
		
	GetDogeOpReturn: An Azure function (lambda) that scans the full dogecoin blockchain and writes out any commands (OP_RETURN)
		in such a way that the client can download this data since downloading from the blockchain itself is really slow.
		
	You can certainly come up with your own ways to parse and download the data from whatever blockchain you want to use.

In this example, we use the OP_RETURN field available in dogecoin to enter simple commmands like

	Create <username>
	Buy Land
	
and by sending transactions to a given account. The account is settable in the __Init object field called TargetWallet in the client.

Each different wallet represents a different game.

Each player will run the entire simulation, and since it's deterministic, they will end up with the same state.
(This is unless System.Random is different on different Unity client implementations. If so, adding a specific prng
like the Mersenne Twister would be needed.)

It is very basic, and slow to get commands in, but work with a blockchain set up for this might make it feasible.

There could also be minigames the players play where they enter a dungeon and perform a bunch of commands, but only those
commands get added to the blockchain and they have to get processed.

There could be sites where the games are checkpointed so people don't have to calculate everything locally.

There could be different "worlds" that get created, then destroyed and allow players to take a bit of a character state
from one world to the next to keep the data from getting too big.

I could imagine a lot of ways to make some interesting games with some interesting realtime gameplay as long as the local gameplay
is deterministic based on some start point, and the person doing the content is willing to wait a bit for their results to get added
back into the blockchain so everything stays deterministic. We could even have PVP since the attacks would be deterministic


Also, there is no server or database. A system is only decentralized as its most centralized part. So, this
system has no private database or private servers. Anyone telling you they have a "web3" thing but they have a login
or their own database for doing stuff is blowing smoke.
