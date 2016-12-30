﻿using PlayerTwo.Models;
using PlayerTwo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerTwo.Services
{
    public class GameStateService : IGameStateService
    {
        private IGameLogReaderService _gameLogReaderService;
        private ICardService _cardService;
        private IGameLogParserService _gameLogParserService;

        private List<Card> _hand;
        private List<Card> _board;
        private List<Card> _opponentBoard;
        private List<Card> _opponentHand;

        public GameStateService(IGameLogReaderService gameLogReaderService, ICardService cardService, IGameLogParserService gameLogParserService)
        {
            _gameLogReaderService = gameLogReaderService;
            _cardService = cardService;
            _gameLogParserService = gameLogParserService;

            _hand = new List<Card>();
            _board = new List<Card>();
            _opponentBoard = new List<Card>();
            _opponentHand = new List<Card>();
        }

        public IEnumerable<Card> GetHand()
        {
            UpdateGameLog();
            return _hand;
        }

        public IEnumerable<Card> GetOpponentHand()
        {
            UpdateGameLog();
            return _opponentHand;
        }

        public IEnumerable<Card> GetBoard()
        {
            UpdateGameLog();
            return _board;
        }

        public IEnumerable<Card> GetOpponentBoard()
        {
            UpdateGameLog();
            return _opponentBoard;
        }

        private void UpdateGameLog()
        {
            var newActions = _gameLogReaderService.GetNewActions();

            if (newActions != null && newActions.Any())
            {
                foreach (var action in newActions)
                {
                    var gameEvent = _gameLogParserService.GetGameEvent(action);

                    if (gameEvent.Type == GameEventType.Card)
                    {
                        Console.WriteLine("Found game event: " + gameEvent.Id);
                        var card = _cardService.GetCard(gameEvent.CardId);

                        if (card != null)
                        {
                            Console.WriteLine("Found game card: " + card.Name);

                            card.GameEventId = gameEvent.Id;

                            HandleOwnDraws(gameEvent, card);
                            HandleOwnBoard(gameEvent, card);
                            HandleOpponentBoard(gameEvent, card);
                        }

                        HandleOpponentDraws(gameEvent);
                    }
                }
            }
        }

        private void HandleOpponentBoard(GameEvent gameEvent, Card card)
        {
            // Opponent is playing or target
            if (gameEvent.ToPlayer == PlayerId.Opponent || gameEvent.PlayerId == PlayerId.Opponent)
            {
                // The card is going to the play zone from hand or dekc
                if (gameEvent.ToZone == Zone.PLAY && (gameEvent.FromZone == Zone.HAND || gameEvent.FromZone == Zone.DECK))
                {
                    _opponentBoard.Add(card);
                } // Check for dead minions
                else if (gameEvent.ToZone == Zone.GRAVEYARD && gameEvent.FromZone == Zone.PLAY)
                {
                    foreach (var boardCard in _opponentBoard)
                    {
                        if (boardCard.GameEventId == gameEvent.Id)
                        {
                            _opponentBoard.Remove(boardCard);
                            break;
                        }
                    }
                }
            }
        }

        private void HandleOwnBoard(GameEvent gameEvent, Card card)
        {
            // We are playing or we are the target of the play
            if (gameEvent.ToPlayer == PlayerId.Self || gameEvent.PlayerId == PlayerId.Self)
            {
                // The card is going to the play zone from hand or dekc
                if (gameEvent.ToZone == Zone.PLAY && (gameEvent.FromZone == Zone.HAND || gameEvent.FromZone == Zone.DECK)) {
                    _board.Add(card);
                } // Check for dead minions
                else if (gameEvent.ToZone == Zone.GRAVEYARD && gameEvent.FromZone == Zone.PLAY)
                {
                    foreach (var boardCard in _board)
                    {
                        if (boardCard.GameEventId == gameEvent.Id)
                        {
                            _board.Remove(boardCard);
                            break;
                        }
                    }
                }
            }
        }

        private void HandleOpponentDraws(GameEvent gameEvent)
        {
            if (gameEvent.PlayerId == PlayerId.Opponent)
            {
                if (gameEvent.ToZone == Zone.HAND)
                {
                    _opponentHand.Add(new Card { GameEventId = gameEvent.Id });
                }
                else if (gameEvent.FromZone == Zone.HAND)
                {
                    foreach (var handCard in _opponentHand)
                    {
                        if (handCard.GameEventId == gameEvent.Id)
                        {
                            _opponentHand.Remove(handCard);
                            break;
                        }
                    }
                }
            }
        }

        private void HandleOwnDraws(GameEvent gameEvent, Card card)
        {
            if (gameEvent.PlayerId == PlayerId.Self)
            {
                if (gameEvent.ToZone == Zone.HAND)
                {
                    _hand.Add(card);
                }
                else if (gameEvent.FromZone == Zone.HAND)
                {
                    foreach (var handCard in _hand)
                    {
                        if (handCard.GameEventId == gameEvent.Id)
                        {
                            _hand.Remove(handCard);
                            break;
                        }
                    }
                }

            }
        }
    }
}
