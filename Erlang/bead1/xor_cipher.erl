%%%=============================================================================
%%% XOR_Cipher code and decode module
%%%=============================================================================

-module(xor_cipher).
-export([toBitString/1, charToBitString/1, bitStringToChar/1]).

%%%=============================================================================
%%% Types
%%%=============================================================================

-type bitString() :: list(0|1).
-type bit() :: 0 | 1.

%%%=============================================================================
%%% API
%%%=============================================================================

%% Converting a decimal number to a bitstring (2 pont)
-spec toBitString(N :: number()) -> bitString().
toBitString(N) -> toBitString(N, []).

%% Converting a character to a bitstring (2 pont)
-spec charToBitString(A :: char()) -> bitString() | {error, atom()}.
charToBitString(A) -> addExtraZeros(toBitString(A)).

%% Converting a bitstring to a character (2 pont)
-spec bitStringToChar(BitStr :: bitString()) -> char().
bitStringToChar(BitStr) ->
	{Result, _} = lists:foldl(fun(Curr, {Sofar, Index}) -> 
			NewVal = case Curr of
				1 -> Sofar + math:pow(2, Index);
				0 -> Sofar
			end,
			{NewVal, Index+1}
			end, {0, 0}, BitStr),
	trunc(Result).

%%%=============================================================================
%%% Internal functions
%%%=============================================================================

-spec toBitString(N :: number(), Acc :: bitString()) -> bitString().
toBitString(0, Acc) -> lists:reverse(Acc);
toBitString(N, Acc) -> toBitString(N div 2, [N rem 2 | Acc]).

-spec addExtraZeros(Acc :: bitString()) -> bitString() | {error, atom()}.
addExtraZeros(Acc) when length(Acc) > 8 ->
	{error, invalid_character};
addExtraZeros(Acc) when length(Acc) == 8 ->	Acc;
addExtraZeros(Acc) -> addExtraZeros(Acc ++ [0]).