using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Constants
{
    public static class GameConfig
    {
        public static class Flow
        {

        }
        public static class Text
        {

        }
        public static class Device
        {
            public const float DEVICE_RATE_DEFAULT = 0.5625f;
        }
        public static class URL
        {
            public const string BUNDLE_ID = "c3.block.games";
            public const string WEB_SOCKET = "wss://c3-multiplay-ezhmzmbjra-df.a.run.app/";
            public const string REST = "https://c3-dev-apim.azure-api.net/";
            public const string BLOCK_GAME = "c3_room";
        }
        public static class Board
        {
            public const int SIZE_X = 10;
            public const int SIZE_Y = 10;
        }
        public static class Combo
        {
            public static int[] SCORES = { 0, 100, 300, 600, 1000, 1500, 2100 };
            public static int WOWBO_COUNT = 1;
        }
        public static class Streak
        {
            public static int STREAK2_SCORE = 250;
            public static int SCORE = 150;
        }
        public static class Time
        {
            public const float MOVING_ON_BOARD = .125f;
            public const float OVER_GAME_GRAY_DELAY = .15f;
            public const long VIBRATION = 250;
        }
        public static class Shake
        {
            public const float DURATION = 0.075f * 3;
            public const float AMT = 0.05f;
        }
        public static class Block
        {
            public const float MINI_SCALE = 0.571f;
            public const float DRAG_DISTANCE = 0.5f;
        }
        public static class Item
        {
            public const float MINI_SIZE = 42f;
            public const float MOVING_SIZE = 70f;
            public static readonly string[] COLORS = { "blue", "cyan", "green", "orange", "pink", "red" };
        }
        public static class Weight
        {
            public const int A = 10;
            public const int B = 50;
            public const int C = 9999;
        }
    }
}