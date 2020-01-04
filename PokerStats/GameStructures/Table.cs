using System;
using System.Collections.Generic;
using System.Text;

namespace PokerStats.GameStructures
{
    public class Table
    {
        public static readonly string[][] BestHoleCardsByPlayerCount = new string[][]
{
            new string[] { }, //Zero players
            new string[] { }, //One player
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "88o", "77o", "AKs", "AQs", "AKo", "AJs", "AQo", "66o", "KQs", "AJo", "A9s", "ATs", "ATo", "KJs", "A8s", "KTs", "55o", "KQo", "A7s", "KJo", "K9s", "A9o", "A6s", "QJs", "KTo", "A8o", "A5s", "K8s", "A7o", "QTs", "A4s", "K9o", "K7s", "44o", "QJo", "Q9s", "A6o", "A5o", "QTo", "K8o", "JTs", "A3s", "A2s", "Q8s", "A4o", "Q9o", "K6s", "K4s", "J9s", "K7o", "K3s", "33o", "K2s", "JTo", "Q6s", "A3o", "K6o", "K5s", "Q8o", "Q7s", "A2o", "J8s", "K5o", "J9o", "T9s", "Q5s", "T8s", "K4o", "Q7o", "J7s", "Q4s", "22o", "K3o", "J8o", "Q6o", "98s", "Q3s", "T9o", "J6s", "Q2s", "J5s", "K2o", "T7s", "Q5o", "T8o", "J4s", "J7o", "Q4o", "J6o", "97s", "T6s", "Q2o", "96s", "J3s", "J2s", "Q3o", "T7o", "T5s", "98o", "J5o", "T4s", "87s", "97o", "T6o", "J3o", "86s", "95s", "J4o", "94s", "T2s", "T3s", "T5o", "J2o", "76s", "96o", "87o", "85s", "84s", "93s", "T4o", "65s", "95o", "T3o", "86o", "92s", "75s", "T2o", "74s", "83s", "85o", "76o", "82s", "94o", "64s", "73s", "93o", "75o", "84o", "54s", "92o", "63s", "65o", "72s", "74o", "53s", "83o", "62s", "43s", "64o", "82o", "52s", "73o", "63o", "42s", "54o", "72o", "53o", "62o", "32s", "52o", "43o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "AKs", "88o", "AQs", "AJs", "AKo", "77o", "ATs", "AQo", "KQs", "KTs", "KJs", "AJo", "66o", "A9s", "QJs", "ATo", "KQo", "A8s", "KJo", "A7s", "QTs", "A9o", "KTo", "55o", "K9s", "QJo", "A6s", "A8o", "K8s", "Q9s", "JTs", "A5s", "QTo", "K9o", "K6s", "A7o", "K7s", "Q8s", "J9s", "44o", "A4s", "A3s", "JTo", "A6o", "A2s", "K5s", "Q9o", "K8o", "T9s", "A5o", "J8s", "K3s", "A4o", "T8s", "Q7s", "Q6s", "J9o", "K4s", "J7s", "Q5s", "K2s", "K7o", "33o", "98s", "K6o", "Q8o", "A3o", "T7s", "T9o", "A2o", "K5o", "97s", "Q4s", "J6s", "J8o", "22o", "Q3s", "K4o", "Q7o", "Q2s", "87s", "J4s", "J5s", "Q6o", "T8o", "T6s", "96s", "J7o", "Q5o", "K3o", "K2o", "98o", "T5s", "76s", "86s", "T7o", "J3s", "95s", "J2s", "Q4o", "T4s", "85s", "97o", "J6o", "T3s", "Q3o", "75s", "J5o", "87o", "T6o", "T2s", "65s", "84s", "74s", "Q2o", "J4o", "94s", "96o", "93s", "64s", "54s", "92s", "J3o", "T5o", "86o", "83s", "76o", "82s", "73s", "95o", "T4o", "J2o", "63s", "53s", "85o", "T3o", "75o", "72s", "65o", "52s", "T2o", "43s", "94o", "62s", "74o", "84o", "64o", "92o", "42s", "93o", "32s", "54o", "63o", "73o", "83o", "82o", "53o", "43o", "72o", "62o", "52o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "99o", "AKs", "AQs", "88o", "AKo", "KQs", "AJs", "KJs", "AQo", "77o", "ATs", "AJo", "QJs", "KQo", "A9s", "KTs", "QTs", "A8s", "ATo", "66o", "K9s", "KJo", "JTs", "A7s", "KTo", "K8s", "A6s", "QJo", "A9o", "Q9s", "A5s", "J9s", "55o", "T9s", "A8o", "Q8s", "QTo", "A4s", "A3s", "K7s", "JTo", "K9o", "44o", "A7o", "J8s", "K6s", "A2s", "K5s", "Q9o", "Q6s", "T8s", "Q7s", "J7s", "98s", "K4s", "A6o", "J9o", "K2s", "K8o", "Q5s", "A5o", "T7s", "K7o", "Q8o", "K3s", "T9o", "87s", "33o", "J6s", "97s", "A4o", "Q4s", "K6o", "A3o", "J8o", "J5s", "Q3s", "Q2s", "A2o", "76s", "T6s", "J4s", "T8o", "Q7o", "86s", "J3s", "22o", "98o", "K5o", "95s", "96s", "J7o", "T5s", "K4o", "J2s", "T4s", "Q6o", "T3s", "K3o", "65s", "75s", "T7o", "Q5o", "85s", "97o", "K2o", "T2s", "74s", "64s", "84s", "J6o", "87o", "94s", "Q4o", "93s", "J5o", "92s", "54s", "Q3o", "T6o", "Q2o", "63s", "83s", "73s", "96o", "J4o", "86o", "76o", "72s", "43s", "53s", "T5o", "82s", "J3o", "95o", "85o", "T4o", "62s", "65o", "J2o", "75o", "52s", "T3o", "42s", "T2o", "84o", "94o", "32s", "74o", "64o", "93o", "92o", "54o", "73o", "63o", "83o", "53o", "82o", "43o", "62o", "72o", "52o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "99o", "AQs", "AKo", "AJs", "KQs", "88o", "ATs", "KJs", "AQo", "KTs", "77o", "QJs", "QTs", "AJo", "A9s", "KQo", "A8s", "K9s", "A7s", "66o", "ATo", "KJo", "JTs", "A6s", "Q9s", "QJo", "KTo", "K8s", "J9s", "A5s", "T9s", "A9o", "55o", "QTo", "K6s", "Q8s", "K7s", "A8o", "A4s", "JTo", "A2s", "K9o", "A3s", "J8s", "T8s", "Q7s", "44o", "K5s", "A7o", "98s", "K4s", "Q6s", "Q9o", "K3s", "J7s", "33o", "T7s", "K8o", "A6o", "97s", "J9o", "K2s", "Q5s", "J6s", "87s", "T9o", "Q4s", "K7o", "A5o", "Q8o", "96s", "T6s", "Q2s", "J5s", "Q3s", "76s", "J8o", "A4o", "J4s", "86s", "T8o", "A3o", "K6o", "T5s", "J3s", "22o", "K5o", "A2o", "85s", "Q7o", "95s", "98o", "J2s", "T4s", "65s", "75s", "T3s", "Q6o", "94s", "K4o", "T2s", "T7o", "J7o", "84s", "64s", "K3o", "92s", "Q5o", "97o", "87o", "54s", "83s", "93s", "K2o", "63s", "74s", "J6o", "Q4o", "73s", "53s", "62s", "86o", "Q3o", "T6o", "82s", "J5o", "72s", "96o", "43s", "76o", "Q2o", "52s", "J4o", "T5o", "42s", "75o", "T4o", "85o", "J3o", "95o", "65o", "J2o", "32s", "T3o", "84o", "64o", "74o", "T2o", "94o", "93o", "54o", "63o", "92o", "73o", "83o", "53o", "82o", "43o", "62o", "72o", "52o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "99o", "AJs", "KQs", "AKo", "88o", "ATs", "KJs", "AQo", "QJs", "KTs", "A9s", "KQo", "AJo", "77o", "QTs", "JTs", "K9s", "A8s", "KJo", "66o", "ATo", "A7s", "Q9s", "A6s", "K8s", "QJo", "J9s", "A5s", "55o", "Q8s", "KTo", "A4s", "K7s", "A9o", "T9s", "QTo", "A3s", "A2s", "J8s", "K6s", "T8s", "JTo", "K5s", "44o", "A8o", "K9o", "Q7s", "Q6s", "K4s", "98s", "K3s", "J7s", "Q9o", "33o", "A7o", "T7s", "J6s", "K2s", "Q5s", "J9o", "Q4s", "97s", "87s", "22o", "T6s", "K8o", "Q3s", "A6o", "T9o", "K7o", "J3s", "J5s", "A5o", "76s", "86s", "J4s", "Q2s", "Q8o", "96s", "95s", "A4o", "T5s", "J2s", "J8o", "65s", "T4s", "85s", "75s", "T3s", "K6o", "A3o", "64s", "94s", "T8o", "98o", "A2o", "T2s", "Q7o", "54s", "K5o", "63s", "74s", "84s", "93s", "J7o", "92s", "53s", "K4o", "Q6o", "T7o", "83s", "87o", "82s", "73s", "K3o", "97o", "43s", "Q5o", "K2o", "62s", "72s", "J6o", "52s", "Q4o", "T6o", "Q3o", "42s", "32s", "96o", "86o", "J5o", "76o", "Q2o", "T5o", "J4o", "J3o", "85o", "65o", "75o", "95o", "J2o", "T4o", "T3o", "64o", "T2o", "84o", "74o", "94o", "54o", "93o", "92o", "63o", "53o", "83o", "73o", "82o", "62o", "43o", "52o", "72o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "AKs", "TTo", "AQs", "99o", "KQs", "AKo", "AJs", "ATs", "KJs", "88o", "QJs", "AQo", "KTs", "A9s", "QTs", "KQo", "77o", "AJo", "A8s", "K9s", "JTs", "A7s", "66o", "Q9s", "ATo", "KJo", "A4s", "J9s", "K8s", "A6s", "A5s", "55o", "QJo", "K7s", "T9s", "Q8s", "KTo", "A3s", "A2s", "T8s", "K6s", "QTo", "44o", "J8s", "A9o", "98s", "K5s", "33o", "K4s", "Q6s", "K2s", "J7s", "JTo", "22o", "Q7s", "K3s", "K9o", "T7s", "Q4s", "A8o", "Q3s", "Q5s", "97s", "J6s", "87s", "A7o", "Q9o", "T6s", "J5s", "Q2s", "76s", "96s", "J9o", "86s", "A6o", "J2s", "J3s", "T9o", "K8o", "T5s", "75s", "J4s", "65s", "A5o", "T3s", "85s", "95s", "T4s", "Q8o", "K7o", "64s", "T2s", "A4o", "93s", "74s", "J8o", "K6o", "54s", "T8o", "84s", "A3o", "92s", "94s", "Q7o", "A2o", "98o", "83s", "63s", "82s", "K5o", "62s", "J7o", "53s", "43s", "73s", "K4o", "72s", "Q6o", "52s", "T7o", "42s", "97o", "K3o", "Q5o", "87o", "32s", "K2o", "J6o", "Q4o", "96o", "T6o", "86o", "Q3o", "76o", "J5o", "Q2o", "J4o", "T5o", "65o", "85o", "J3o", "75o", "95o", "J2o", "T4o", "64o", "T3o", "T2o", "84o", "74o", "54o", "94o", "93o", "53o", "63o", "92o", "73o", "82o", "83o", "43o", "62o", "52o", "72o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "AKs", "TTo", "AQs", "AJs", "KQs", "99o", "AKo", "ATs", "KJs", "88o", "KTs", "QJs", "A9s", "AQo", "77o", "QTs", "JTs", "KQo", "A8s", "AJo", "K9s", "66o", "A7s", "A6s", "Q9s", "A5s", "55o", "J9s", "K8s", "KJo", "ATo", "A4s", "T9s", "K7s", "A3s", "A2s", "44o", "33o", "K6s", "QJo", "KTo", "J8s", "Q8s", "Q7s", "22o", "K5s", "K4s", "T8s", "K2s", "K3s", "98s", "QTo", "J7s", "Q6s", "A9o", "T7s", "Q5s", "JTo", "Q3s", "97s", "J6s", "Q4s", "A8o", "87s", "86s", "K9o", "Q2s", "T6s", "J5s", "76s", "96s", "A7o", "J4s", "T4s", "65s", "Q9o", "J3s", "J2s", "95s", "J9o", "75s", "T5s", "K8o", "T9o", "A6o", "85s", "T3s", "54s", "64s", "94s", "T2s", "74s", "Q8o", "93s", "A5o", "84s", "A4o", "92s", "K7o", "63s", "53s", "83s", "J8o", "73s", "43s", "A3o", "A2o", "K6o", "T8o", "62s", "82s", "72s", "98o", "52s", "42s", "Q7o", "K5o", "32s", "K4o", "J7o", "K3o", "T7o", "97o", "Q6o", "K2o", "87o", "Q5o", "Q4o", "76o", "J6o", "T6o", "86o", "Q3o", "Q2o", "96o", "J5o", "75o", "J4o", "65o", "T5o", "J2o", "85o", "J3o", "T4o", "64o", "95o", "54o", "74o", "T3o", "T2o", "84o", "94o", "63o", "93o", "53o", "73o", "92o", "83o", "43o", "82o", "52o", "62o", "72o", "42o", "32o" },
            new string[] { "AAo", "KKo", "QQo", "JJo", "AKs", "TTo", "AQs", "99o", "AJs", "KQs", "ATs", "AKo", "KJs", "88o", "QJs", "KTs", "77o", "AQo", "A9s", "QTs", "K9s", "A8s", "66o", "JTs", "KQo", "A7s", "AJo", "55o", "Q9s", "44o", "K8s", "A5s", "A6s", "33o", "A3s", "A4s", "J9s", "K7s", "22o", "A2s", "ATo", "KJo", "Q8s", "T9s", "T8s", "K5s", "K6s", "J8s", "QJo", "Q7s", "K3s", "K2s", "K4s", "KTo", "Q6s", "98s", "J7s", "97s", "Q5s", "QTo", "T7s", "A9o", "JTo", "Q4s", "87s", "Q2s", "Q3s", "J6s", "T6s", "A8o", "65s", "K9o", "76s", "96s", "86s", "J5s", "J3s", "J4s", "T5s", "64s", "J2s", "75s", "T3s", "95s", "A7o", "Q9o", "T2s", "T4s", "54s", "85s", "74s", "94s", "K8o", "53s", "A6o", "T9o", "J9o", "84s", "93s", "A5o", "92s", "63s", "73s", "43s", "82s", "A4o", "83s", "Q8o", "62s", "A3o", "K7o", "52s", "J8o", "72s", "A2o", "T8o", "42s", "K6o", "32s", "98o", "Q7o", "K5o", "K4o", "T7o", "J7o", "87o", "97o", "Q6o", "K3o", "K2o", "Q5o", "76o", "Q4o", "86o", "96o", "Q3o", "J6o", "T6o", "Q2o", "J5o", "65o", "75o", "85o", "64o", "J4o", "95o", "J3o", "T5o", "J2o", "T4o", "74o", "54o", "T3o", "T2o", "84o", "63o", "94o", "53o", "43o", "93o", "92o", "73o", "82o", "62o", "83o", "42o", "52o", "72o", "32o" },
};

        public static readonly int[][] BestHoleCardsNumericByPlayerCount = new int[][]
        {
            new int[0],
            new int[0],
            new int[] { 336, 308, 280, 252, 224, 196, 168, 140, 335, 333, 334, 331, 332, 112, 307, 330, 327, 329, 328, 305, 325, 303, 84, 306, 323, 304, 301, 326, 321, 279, 302, 324, 319, 299, 322, 277, 317, 300, 297, 56, 278, 275, 320, 318, 276, 298, 251, 315, 313, 273, 316, 274, 295, 291, 249, 296, 289, 28, 287, 250, 269, 314, 294, 293, 272, 271, 312, 247, 292, 248, 223, 267, 221, 290, 270, 245, 265, 0, 288, 246, 268, 195, 263, 222, 243, 261, 241, 286, 219, 266, 220, 239, 244, 264, 242, 193, 217, 260, 191, 237, 235, 262, 218, 215, 194, 240, 213, 167, 192, 216, 236, 165, 189, 238, 187, 209, 211, 214, 234, 139, 190, 166, 163, 161, 185, 212, 111, 188, 210, 164, 183, 137, 208, 135, 159, 162, 138, 157, 186, 109, 133, 184, 136, 160, 83, 182, 107, 110, 131, 134, 81, 158, 105, 55, 108, 156, 79, 132, 106, 53, 82, 130, 80, 104, 27, 78, 54, 52, 26 },
            new int[] { 336, 308, 280, 252, 224, 196, 335, 168, 333, 331, 334, 140, 329, 332, 307, 303, 305, 330, 112, 327, 279, 328, 306, 325, 304, 323, 277, 326, 302, 84, 301, 278, 321, 324, 299, 275, 251, 319, 276, 300, 295, 322, 297, 273, 249, 56, 317, 315, 250, 320, 313, 293, 274, 298, 223, 318, 247, 289, 316, 221, 271, 269, 248, 291, 245, 267, 287, 296, 28, 195, 294, 272, 314, 219, 222, 312, 292, 193, 265, 243, 246, 0, 263, 290, 270, 261, 167, 239, 241, 268, 220, 217, 191, 244, 266, 288, 286, 194, 215, 139, 165, 218, 237, 189, 235, 264, 213, 163, 192, 242, 211, 262, 137, 240, 166, 216, 209, 111, 161, 135, 260, 238, 187, 190, 185, 109, 83, 183, 236, 214, 164, 159, 138, 157, 133, 188, 212, 234, 107, 81, 162, 210, 136, 131, 110, 79, 208, 55, 186, 105, 134, 160, 108, 182, 53, 184, 27, 82, 106, 132, 158, 156, 80, 54, 130, 104, 78, 52, 26 },
            new int[] { 336, 308, 280, 252, 224, 196, 335, 333, 168, 334, 307, 331, 305, 332, 140, 329, 330, 279, 306, 327, 303, 277, 325, 328, 112, 301, 304, 251, 323, 302, 299, 321, 278, 326, 275, 319, 249, 84, 223, 324, 273, 276, 317, 315, 297, 250, 300, 56, 322, 247, 295, 313, 293, 274, 269, 221, 271, 245, 195, 291, 320, 248, 287, 298, 267, 318, 219, 296, 272, 289, 222, 167, 28, 243, 193, 316, 265, 294, 314, 246, 241, 263, 261, 312, 139, 217, 239, 220, 270, 165, 237, 0, 194, 292, 189, 191, 244, 215, 290, 235, 213, 268, 211, 288, 111, 137, 218, 266, 163, 192, 286, 209, 135, 109, 161, 242, 166, 187, 264, 185, 240, 183, 83, 262, 216, 260, 107, 159, 133, 190, 238, 164, 138, 131, 55, 81, 214, 157, 236, 188, 162, 212, 105, 110, 234, 136, 79, 210, 53, 208, 160, 186, 27, 134, 108, 184, 182, 82, 132, 106, 158, 80, 156, 54, 104, 130, 78, 52, 26 },
            new int[] { 336, 308, 280, 252, 224, 335, 196, 333, 334, 331, 307, 168, 329, 305, 332, 303, 140, 279, 277, 330, 327, 306, 325, 301, 323, 112, 328, 304, 251, 321, 275, 278, 302, 299, 249, 319, 223, 326, 84, 276, 295, 273, 297, 324, 317, 250, 313, 300, 315, 247, 221, 271, 56, 293, 322, 195, 291, 269, 274, 289, 245, 28, 219, 298, 320, 193, 248, 287, 267, 243, 167, 222, 265, 296, 318, 272, 191, 217, 261, 241, 263, 139, 246, 316, 239, 165, 220, 314, 294, 215, 237, 0, 292, 312, 163, 270, 189, 194, 235, 213, 111, 137, 211, 268, 187, 290, 209, 218, 244, 161, 109, 288, 183, 266, 192, 166, 83, 159, 185, 286, 107, 135, 242, 264, 133, 81, 105, 164, 262, 216, 157, 240, 131, 190, 55, 138, 260, 79, 238, 214, 53, 136, 212, 162, 236, 188, 110, 234, 27, 210, 160, 108, 134, 208, 186, 184, 82, 106, 182, 132, 158, 80, 156, 54, 104, 130, 78, 52, 26 },
            new int[] { 336, 308, 280, 252, 224, 335, 333, 196, 331, 307, 334, 168, 329, 305, 332, 279, 303, 327, 306, 330, 140, 277, 251, 301, 325, 304, 112, 328, 323, 275, 321, 299, 278, 249, 319, 84, 273, 302, 317, 297, 326, 223, 276, 315, 313, 247, 295, 221, 250, 293, 56, 324, 300, 271, 269, 291, 195, 289, 245, 274, 28, 322, 219, 243, 287, 267, 248, 265, 193, 167, 0, 217, 298, 263, 320, 222, 296, 237, 241, 318, 139, 165, 239, 261, 272, 191, 189, 316, 215, 235, 246, 111, 213, 163, 137, 211, 294, 314, 109, 187, 220, 194, 312, 209, 270, 83, 292, 107, 135, 161, 185, 244, 183, 81, 290, 268, 218, 159, 166, 157, 133, 288, 192, 55, 266, 286, 105, 131, 242, 79, 264, 216, 262, 53, 27, 190, 164, 240, 138, 260, 214, 238, 236, 162, 110, 136, 188, 234, 212, 210, 108, 208, 160, 134, 186, 82, 184, 182, 106, 80, 158, 132, 156, 104, 54, 78, 130, 52, 26 },
            new int[] { 336, 308, 280, 252, 335, 224, 333, 196, 307, 334, 331, 329, 305, 168, 279, 332, 303, 327, 277, 306, 140, 330, 325, 301, 251, 323, 112, 275, 328, 304, 317, 249, 299, 321, 319, 84, 278, 297, 223, 273, 302, 315, 313, 221, 295, 276, 56, 247, 326, 195, 293, 28, 291, 269, 287, 245, 250, 0, 271, 289, 300, 219, 265, 324, 263, 267, 193, 243, 167, 322, 274, 217, 241, 261, 139, 191, 248, 165, 320, 235, 237, 222, 298, 215, 137, 239, 111, 318, 211, 163, 189, 213, 272, 296, 109, 209, 316, 185, 135, 246, 294, 83, 220, 161, 314, 183, 187, 270, 312, 194, 159, 107, 157, 292, 105, 244, 81, 55, 133, 290, 131, 268, 79, 218, 53, 192, 288, 266, 166, 27, 286, 242, 264, 190, 216, 164, 262, 138, 240, 260, 238, 214, 110, 162, 236, 136, 188, 234, 212, 108, 210, 208, 160, 134, 82, 186, 184, 80, 106, 182, 132, 156, 158, 54, 104, 78, 130, 52, 26 },
            new int[] { 336, 308, 280, 252, 335, 224, 333, 331, 307, 196, 334, 329, 305, 168, 303, 279, 327, 332, 140, 277, 251, 306, 325, 330, 301, 112, 323, 321, 275, 319, 84, 249, 299, 304, 328, 317, 223, 297, 315, 313, 56, 28, 295, 278, 302, 247, 273, 271, 0, 293, 291, 221, 287, 289, 195, 276, 245, 269, 326, 219, 267, 250, 263, 193, 243, 265, 324, 167, 165, 300, 261, 217, 241, 139, 191, 322, 239, 213, 111, 274, 237, 235, 189, 248, 137, 215, 298, 222, 320, 163, 211, 83, 109, 187, 209, 135, 272, 185, 318, 161, 316, 183, 296, 107, 81, 159, 246, 133, 55, 314, 312, 294, 220, 105, 157, 131, 194, 79, 53, 270, 292, 27, 290, 244, 288, 218, 192, 268, 286, 166, 266, 264, 138, 242, 216, 164, 262, 260, 190, 240, 136, 238, 110, 214, 234, 162, 236, 212, 108, 188, 82, 134, 210, 208, 160, 186, 106, 184, 80, 132, 182, 158, 54, 156, 78, 104, 130, 52, 26 },
            new int[] { 336, 308, 280, 252, 335, 224, 333, 196, 307, 331, 334, 305, 329, 168, 279, 303, 327, 140, 332, 112, 277, 251, 325, 301, 84, 306, 323, 56, 330, 321, 299, 275, 223, 317, 28, 315, 319, 249, 313, 328, 304, 0, 297, 273, 278, 295, 293, 221, 247, 195, 291, 245, 271, 302, 219, 289, 287, 269, 265, 267, 193, 276, 326, 263, 250, 243, 165, 167, 191, 261, 217, 300, 241, 324, 235, 139, 111, 239, 237, 137, 189, 215, 213, 163, 109, 211, 274, 322, 209, 187, 81, 83, 248, 135, 185, 183, 320, 161, 222, 298, 107, 159, 133, 55, 318, 316, 131, 105, 296, 272, 79, 53, 157, 246, 314, 220, 27, 312, 294, 194, 292, 270, 290, 218, 288, 286, 166, 268, 244, 192, 266, 138, 264, 164, 242, 190, 260, 262, 216, 136, 110, 240, 108, 238, 162, 236, 214, 188, 134, 212, 234, 210, 82, 208, 80, 106, 160, 186, 184, 54, 182, 132, 158, 104, 156, 52, 130, 78, 26 },
    };

        public static int[] BestHoleCardsNumericHero;
        public static int[] BestHoleCardsNumericVillain;

        public static readonly string[] EmotionalCards = new string[]
        {
            "AAo", "KKo", "QQo", "JJo", "TTo", "AKs", "AQs", "AKo", "99o", "AJs", "ATs", "KQs", "AQo", "KJs", "KTs", "A9s", "88o", "AJo", "QJs", "ATo", "KQo", "A8s", "QTs", "77o", "JTs", "K9s", "A7s", "KJo", "66o", "KTo", "A6s", "K8s", "QJo", "J9s", "QTo", "T9s", "JTo", "98s", "87s", "76s"
        };

        public static readonly int[] EmotionalCardsNumeric = new int[] { 160, 140, 120, 100, 80, 159, 157, 158, 60, 155, 153, 139, 156, 137, 135, 151, 40, 154, 119, 152, 138, 149, 117, 20, 99, 133, 147, 136, 0, 134, 145, 131, 118, 97, 116, 79, 98, 59, 39, 19 };

        private readonly Random NumberGenerator = new Random((int)DateTime.Now.Ticks);
        private long[][] HoleCardsWinCounter;
        private long[][] HoleCardsDealtCounter;
        private long[][] HoleCardsTieCounter;

        private readonly Dictionary<int, int[]> HandsMadeCount = new Dictionary<int, int[]>();
        private readonly Dictionary<int, int[]> HandsWonCount = new Dictionary<int, int[]>();
        private readonly Dictionary<int, int[]> HandsTiedCount = new Dictionary<int, int[]>();

        private readonly Dictionary<long, int> HandRankCount = new Dictionary<long, int>();
        private readonly Dictionary<long, PokerHand[]> HandsWithRank = new Dictionary<long, PokerHand[]>();
        private readonly Dictionary<int, double> HoleCardWinRate = new Dictionary<int, double>();

        private Card[] Deck;
        private Card[] CommunityCards = new Card[5];
        private Card[][] PlayerHoleCards;
        private PokerHand[] AllPlayerFullHands;

        private readonly int PlayerCount;
        private readonly bool ManiacPlay;
        public readonly bool LogPokerHandResults;

        private readonly int KeepTopPercentHero;
        private readonly int KeepTopPercentVillain;

        public readonly int DeckNumericValueCount;
        public readonly int DeckSuitCount;

        public Table(int numPlayers, bool maniacPlay, int keepTopPercentHero, int keepTopPercentVillain, bool logPokerHandResults)
        {
            DeckNumericValueCount = 13;
            DeckSuitCount = 4;

            PlayerCount = numPlayers;
            ManiacPlay = maniacPlay;
            LogPokerHandResults = logPokerHandResults;

            KeepTopPercentHero = keepTopPercentHero;
            KeepTopPercentVillain = keepTopPercentVillain;

            //Sort the emotional cards by numeric value.
            Array.Sort(EmotionalCardsNumeric);

            //Sort the Hero's starting hands.
            var bestStartingHands = BestHoleCardsNumericByPlayerCount[PlayerCount];
            var bestStartingHandHeroCount = (int)(bestStartingHands.Length * ((double)keepTopPercentHero / 100));
            BestHoleCardsNumericHero = new int[bestStartingHandHeroCount];
            Array.Copy(bestStartingHands, BestHoleCardsNumericHero, bestStartingHandHeroCount);
            Array.Sort(BestHoleCardsNumericHero);

            //Sort the Villain's starting hands.
            var bestStartingHandVillainCount = (int)(bestStartingHands.Length * ((double)keepTopPercentVillain / 100));
            BestHoleCardsNumericVillain = new int[bestStartingHandVillainCount];
            Array.Copy(bestStartingHands, BestHoleCardsNumericVillain, bestStartingHandVillainCount);
            Array.Sort(BestHoleCardsNumericVillain);

            //Sort the best starting hands by numeric value.
            for (int i = 2; i < BestHoleCardsNumericByPlayerCount.Length; i++)
            {
                Array.Sort(BestHoleCardsNumericByPlayerCount[i]);
            }

            HoleCardsWinCounter = new long[DeckNumericValueCount][];
            for (int i = 0; i < HoleCardsWinCounter.Length; i++)
            {
                HoleCardsWinCounter[i] = new long[DeckNumericValueCount];
            }

            HoleCardsDealtCounter = new long[DeckNumericValueCount][];
            for (int i = 0; i < HoleCardsDealtCounter.Length; i++)
            {
                HoleCardsDealtCounter[i] = new long[DeckNumericValueCount];
            }

            HoleCardsTieCounter = new long[DeckNumericValueCount][];
            for (int i = 0; i < HoleCardsTieCounter.Length; i++)
            {
                HoleCardsTieCounter[i] = new long[DeckNumericValueCount];
            }

            //Populate the deck
            Deck = new Card[DeckSuitCount * DeckNumericValueCount];
            for (short suit = 0; suit < DeckSuitCount; suit++)
            {
                for (short value = 0; value < DeckNumericValueCount; value++)
                {
                    int slot = suit * DeckNumericValueCount + value;
                    Deck[slot] = new Card(suit, value);
                }
            }

            PlayerHoleCards = new Card[numPlayers][];
            for (int i = 0; i < PlayerHoleCards.Length; i++)
            {
                PlayerHoleCards[i] = new Card[2];
            }

            AllPlayerFullHands = new PokerHand[numPlayers];
            for (int i = 0; i < numPlayers; i++)
            {
                AllPlayerFullHands[i] = new PokerHand(this);
            }
        }

        public void PlayHand(bool isVillainEmotional)
        {
            ShuffleDeck();

            DealCardsToPlayers();

            //Populate community cards.
            GetCommunityCards();

            //Generate the 7-card poker hands.
            GeneratePokerHands(isVillainEmotional, KeepTopPercentHero, KeepTopPercentVillain);

            //Log each hand's play.
            LogHandResults();
        }

        public void PlayHands(long iterations, bool isVillainEmotional)
        {
            for (long i = 0L; i < iterations; i++)
            {
                PlayHand(isVillainEmotional);

                if (i % 100_000 == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Player Count: { PlayerCount } - Hands Remaining: {iterations - i}");
                }
            }
        }

        public void PrintHoleCardWinRatesRankedByBest(string holeCardsStringInput = "")
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            var cardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(cardNumericArray, 0);

            var holeCardsInput = PokerHand.ConvertCardStringToNumericValue(holeCardsStringInput, this);

            var pos = -1;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                pos++;

                if (holeCardsInput != 0 && holeCardsInput != holeCards.Key) continue;

                var winRate = holeCardsAndWinRates[holeCards.Key];

                cardNumericArray[pos] = holeCards.Key;
                winRateArray[pos] = winRate[0];
            }

            Array.Sort(winRateArray, cardNumericArray);
            Array.Reverse(winRateArray);
            Array.Reverse(cardNumericArray);

            StringBuilder sb = new StringBuilder();

            var ratePaddingLength = 12;

            sb.AppendLine("Hole Cards".PadRight(ratePaddingLength) + "Win Rate".PadRight(ratePaddingLength) + "Loss Rate".PadRight(ratePaddingLength) + "Tie Rate");
            for (int i = 0; i < cardNumericArray.Length; i++)
            {
                var holeCardsNumeric = cardNumericArray[i];

                if (holeCardsInput != 0 && holeCardsNumeric != holeCardsInput) continue;

                var winRate = holeCardsAndWinRates[holeCardsNumeric][0];
                var lossRate = holeCardsAndWinRates[holeCardsNumeric][1];
                var tieRate = holeCardsAndWinRates[holeCardsNumeric][2];

                if (winRate == 0.0) continue;

                var holeCardsString = PokerHand.ConvertHoleCardsNumericValueToString(holeCardsNumeric, this);

                sb.AppendLine($"{holeCardsString.PadRight(ratePaddingLength)}{winRate.ToString("0.0000").PadRight(ratePaddingLength)}{lossRate.ToString("0.0000").PadRight(ratePaddingLength)}{tieRate.ToString("0.0000")}");
            }
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }

        public void PrintWinRatesForPokerHandsMade(string holeCardsStringInput = "")
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            var holeCardsArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardsArray, 0);

            var holeCardsInput = PokerHand.ConvertCardStringToNumericValue(holeCardsStringInput, this);

            var holeCardsCount = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {
                if (holeCardsInput != 0 && holeCards.Key != holeCardsInput) continue;

                // [win rate, loss rate, tie rate]
                var holeCardsResultStats = holeCardsAndWinRates[holeCards.Key];

                if (holeCardsResultStats[0] == 0.0) continue;

                holeCardsArray[holeCardsCount] = holeCards.Key;
                winRateArray[holeCardsCount] = holeCardsResultStats[0];

                holeCardsCount++;
            }

            Array.Sort(winRateArray, holeCardsArray);
            Array.Reverse(winRateArray);
            Array.Reverse(holeCardsArray);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine();

            for (int j = 0; j < holeCardsCount; j++)
            {
                var targetHoleCards = holeCardsArray[j];

                if (holeCardsInput != 0 && targetHoleCards != holeCardsInput) continue;

                var handsMadeArray = HandsMadeCount[targetHoleCards];
                var handsWonArray = HandsWonCount[targetHoleCards];
                var handsTiedArray = HandsTiedCount[targetHoleCards];

                var totalCardAppearances = 0;
                for (int i = 0; i < handsMadeArray.Length; i++)
                {
                    totalCardAppearances += handsMadeArray[i];
                }

                var targetCardString = PokerHand.ConvertHoleCardsNumericValueToString(targetHoleCards, this);

                var targetCardPadding = 8;
                var rankPadding = 18;
                var rankChancePadding = 15;
                var rankCountPadding = 12;
                var rankWinCountPadding = 17;
                var rankTieCountPadding = 17;
                var rankWinRatePadding = 16;
                var rankTieRatePadding = 16;

                sb.Append(targetCardString.PadRight(targetCardPadding));
                sb.Append("Rank".PadRight(rankPadding));
                sb.Append("Rank Chance".PadRight(rankChancePadding));
                sb.Append("Rank Count".PadRight(rankCountPadding));
                sb.Append("Rank Win Count".PadRight(rankWinCountPadding));
                sb.Append("Rank Tie Count".PadRight(rankTieCountPadding));
                sb.Append("Rank Win Rate".PadRight(rankWinRatePadding));
                sb.Append("Rank Tie Rate".PadRight(rankTieRatePadding));
                sb.Append("Rank Win Index");
                sb.AppendLine();

                for (int i = 1; i < handsMadeArray.Length; i++)
                {
                    var handRank = PokerHand.HandRanks[i].PadRight(rankPadding);

                    var handRankPossibility = (totalCardAppearances == 0 ? 0 : (double)handsMadeArray[i] / totalCardAppearances);
                    var handRankPossibilityString = $"{handRankPossibility.ToString("0.0000")}".PadRight(rankChancePadding);
                    var handRankCount = $"{handsMadeArray[i]}".PadRight(rankCountPadding);
                    var handWinCount = $"{handsWonArray[i]}".PadRight(rankWinCountPadding);
                    var handTieCount = $"{handsTiedArray[i]}".PadRight(rankTieCountPadding);

                    var handWinRate = (handsMadeArray[i] == 0 ? 0 : (double)handsWonArray[i] / handsMadeArray[i]);
                    var handWinRateString = $"{handWinRate.ToString("0.0000")}".PadRight(rankWinRatePadding);

                    var handTieRate = (handsMadeArray[i] == 0 ? 0 : (double)handsTiedArray[i] / handsMadeArray[i]);
                    var handTieRateString = $"{handTieRate.ToString("0.0000")}".PadRight(rankTieRatePadding);

                    var winIndex = handRankPossibility * (handWinRate + handTieRate);
                    var winIndexString = $"{winIndex.ToString("0.0000")}";

                    sb.Append("".PadLeft(targetCardPadding));
                    sb.Append(handRank);
                    sb.Append(handRankPossibilityString);
                    sb.Append(handRankCount);
                    sb.Append(handWinCount);
                    sb.Append(handTieCount);
                    sb.Append(handWinRateString);
                    sb.Append(handTieRateString);
                    sb.Append(winIndexString);
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        public void PrintHoleCardsNumericRankedByBestForArray()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();


            var holeCardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardNumericArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {

                var holeCardsNumeric = holeCards.Key;
                var winRate = holeCardsAndWinRates[holeCardsNumeric];

                winRateArray[pos] = winRate[0];

                holeCardNumericArray[pos] = holeCardsNumeric;

                pos++;
            }

            Array.Sort(winRateArray, holeCardNumericArray);
            Array.Reverse(holeCardNumericArray);

            var holeCardArray = new string[holeCardsAndWinRates.Count];
            for (var i = 0; i < holeCardNumericArray.Length; i++)
            {
                holeCardArray[i] = holeCardNumericArray[i].ToString();
            }

            sb.Append("new int[] { ");
            sb.Append(string.Join(", ", holeCardArray));
            sb.Append(" },");

            Console.WriteLine(sb.ToString());
        }

        public void PrintHoleCardsRankedByBestForArray()
        {
            var holeCardsAndWinRates = GetHoleCardsAndWinRates();

            StringBuilder sb = new StringBuilder();


            var holeCardNumericArray = new int[holeCardsAndWinRates.Count];
            var winRateArray = new double[holeCardsAndWinRates.Count];
            holeCardsAndWinRates.Keys.CopyTo(holeCardNumericArray, 0);

            var pos = 0;
            foreach (var holeCards in holeCardsAndWinRates)
            {

                var holeCardsNumeric = holeCards.Key;
                var winRate = holeCardsAndWinRates[holeCardsNumeric];

                winRateArray[pos] = winRate[0];

                holeCardNumericArray[pos] = holeCardsNumeric;

                pos++;
            }

            Array.Sort(winRateArray, holeCardNumericArray);
            Array.Reverse(holeCardNumericArray);

            var holeCardArray = new string[holeCardsAndWinRates.Count];
            for (var i = 0; i < holeCardNumericArray.Length; i++)
            {
                holeCardArray[i] = "\"" + PokerHand.ConvertHoleCardsNumericValueToString(holeCardNumericArray[i], this) + "\"";
            }

            sb.Append("new string[] { ");
            sb.Append(string.Join(", ", holeCardArray));
            sb.Append(" },");

            Console.WriteLine(sb.ToString());
        }

        private Dictionary<int, double[]> GetHoleCardsAndWinRates()
        {
            var holeCardsAndWinRates = new Dictionary<int, double[]>();

            for (int smallCardValue = 0; smallCardValue < HoleCardsWinCounter.Length; smallCardValue++)
            {
                for (int bigCardValue = smallCardValue; bigCardValue < HoleCardsWinCounter[0].Length; bigCardValue++)
                {
                    var dealCount = HoleCardsDealtCounter[smallCardValue][bigCardValue];
                    var winCount = HoleCardsWinCounter[smallCardValue][bigCardValue];
                    var tieCount = HoleCardsTieCounter[smallCardValue][bigCardValue];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;

                    var holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(bigCardValue, smallCardValue, false, this);


                    holeCardsAndWinRates.Add(holeCardsNumeric, new double[] { winRate, lossRate, tieRate });
                }
            }

            for (int bigCardValue = 0; bigCardValue < HoleCardsWinCounter.Length; bigCardValue++)
            {
                for (int smallcardValue = 0; smallcardValue < bigCardValue; smallcardValue++)
                {
                    var dealCount = HoleCardsDealtCounter[bigCardValue][smallcardValue];
                    var winCount = HoleCardsWinCounter[bigCardValue][smallcardValue];
                    var tieCount = HoleCardsTieCounter[bigCardValue][smallcardValue];
                    var lossCount = dealCount - winCount - tieCount;

                    var winRate = dealCount == 0 ? 0 : (double)winCount / dealCount;
                    var tieRate = dealCount == 0 ? 0 : (double)tieCount / dealCount;
                    var lossRate = dealCount == 0 ? 0 : (double)lossCount / dealCount;

                    var holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(bigCardValue, smallcardValue, true, this);

                    holeCardsAndWinRates.Add(holeCardsNumeric, new double[] { winRate, lossRate, tieRate });
                }
            }

            return holeCardsAndWinRates;
        }

        private void DealCardsToPlayers()
        {
            //Deal cards
            for (byte i = 0; i < PlayerHoleCards.Length * PlayerHoleCards[0].Length; i++)
            {
                var player = i % PlayerHoleCards.Length;
                var holeCard = i / PlayerHoleCards.Length;

                PlayerHoleCards[player][holeCard] = Deck[i];
            }
        }

        private void GetCommunityCards()
        {
            int numCardsDealtToPlayers = PlayerHoleCards.Length * PlayerHoleCards[0].Length;

            Array.Copy(Deck, numCardsDealtToPlayers, CommunityCards, 0, 5);

            //Sort community cards in descending order
            Array.Sort(CommunityCards);
            Array.Reverse(CommunityCards);
        }

        private void GeneratePokerHands(bool isVillainEmotional, int keepTopPercentHero, int keepTopPercentVillain)
        {
            for (int i = 0; i < PlayerHoleCards.Length; i++)
            {
                var playerHoleCards = PlayerHoleCards[i];

                var firstHoleCard = playerHoleCards[0];
                var secondHoleCard = playerHoleCards[1];

                if (firstHoleCard.Value < secondHoleCard.Value)
                {
                    playerHoleCards[0] = secondHoleCard;
                    playerHoleCards[1] = firstHoleCard;
                }

                AllPlayerFullHands[i].GeneratePokerHand(PlayerHoleCards[i], CommunityCards, PlayerCount, isVillainEmotional, keepTopPercentHero, keepTopPercentVillain, ManiacPlay);
            }
        }

        private void LogHandResults()
        {
            Card biggestHoleCard;
            Card smallestHoleCard;
            int holeCardsNumeric;

            if (!ManiacPlay)
            {
                //Log the hand only if there exists a hand the hero would play.
                var hasHeroHand = false;
                foreach (var hand in AllPlayerFullHands)
                {
                    if (hand.IsLiveAsHero)
                    {
                        hasHeroHand = true;
                        break;
                    }
                }
                if (!hasHeroHand) return;
            }

            bool isSuited;

            //Log all hands made. Even the folded ones.
            for (int handIndex = 0; handIndex < AllPlayerFullHands.Length; handIndex++)
            {
                var hand = AllPlayerFullHands[handIndex];

                //Biggest card first
                biggestHoleCard = hand.HoleCards[0];
                smallestHoleCard = hand.HoleCards[1];

                isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
                holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited, this);

                //Determines which side of hole card matrix to log the cards.
                //One side of the diagonal represents suited holde cards. The 
                //diagonal itself and the other side represents unsuited hole
                //cards.
                if (isSuited)
                {
                    HoleCardsDealtCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsDealtCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }

                if (LogPokerHandResults)
                {
                    if (!HandsMadeCount.ContainsKey(holeCardsNumeric))
                    {
                        HandsMadeCount.Add(holeCardsNumeric, new int[10]);
                        HandsWonCount.Add(holeCardsNumeric, new int[10]);
                        HandsTiedCount.Add(holeCardsNumeric, new int[10]);
                    }

                    var handRank = hand.HandRank / 100_000_000_000L;

                    HandsMadeCount[holeCardsNumeric][handRank]++;
                }
            }

            //Determine the strongest hand among the live hands as villains.
            PokerHand strongestHand = null;
            PokerHand strongestHandForHero = null;
            HandRankCount.Clear();
            for (int handIndex = 0; handIndex < AllPlayerFullHands.Length; handIndex++)
            {
                var hand = AllPlayerFullHands[handIndex];

                if (!ManiacPlay && !hand.IsLiveAsVillain) continue;

                var handRank = hand.HandRank;
                if (!HandRankCount.ContainsKey(handRank))
                {
                    HandRankCount.Add(handRank, 0);
                }

                if (LogPokerHandResults)
                {
                    if (!HandsWithRank.ContainsKey(handRank))
                    {
                        HandsWithRank.Add(handRank, new PokerHand[PlayerCount]);
                    }
                    HandsWithRank[handRank][HandRankCount[handRank]] = hand;
                }

                HandRankCount[handRank]++;

                if (!ManiacPlay && hand.IsLiveAsHero)
                {
                    if (strongestHandForHero == null || handRank > strongestHandForHero.HandRank)
                    {
                        strongestHandForHero = hand;
                    }
                }

                if (strongestHand == null || handRank > strongestHand.HandRank)
                {
                    strongestHand = hand;
                }
            }

            //If none of the hands meet the Villain Starting Hands criteria, do not log this hand.
            if (strongestHand == null)
            {
                return;
            }

            //If there is no hand that the hero would play, then do not log a win or tie.
            if (!ManiacPlay && strongestHandForHero == null)
            {
                return;
            }

            //If the hero loses, do not log the hand as a win or tie.
            if (!ManiacPlay && strongestHand.HandRank != strongestHandForHero.HandRank)
            {
                return;
            }

            //Check for tie
            var isTie = HandRankCount[strongestHand.HandRank] > 1;

            //Biggest card first
            biggestHoleCard = strongestHand.HoleCards[0];
            smallestHoleCard = strongestHand.HoleCards[1];

            isSuited = biggestHoleCard.Suit == smallestHoleCard.Suit;
            holeCardsNumeric = PokerHand.ConvertHoleCardsToNumericValue(biggestHoleCard.Value, smallestHoleCard.Value, isSuited, this);

            if (isTie)
            {
                if (isSuited)
                {
                    HoleCardsTieCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsTieCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }
            }
            else
            {
                if (biggestHoleCard.Suit == smallestHoleCard.Suit)
                {
                    HoleCardsWinCounter[biggestHoleCard.Value][smallestHoleCard.Value]++;
                }
                else
                {
                    HoleCardsWinCounter[smallestHoleCard.Value][biggestHoleCard.Value]++;
                }
            }

            if (LogPokerHandResults)
            {
                if (isTie)
                {
                    //Mark all tied hands as a win
                    var tieingPokerHands = HandsWithRank[strongestHand.HandRank];
                    for (int i = 0; i < HandRankCount[strongestHand.HandRank]; i++)
                    {
                        var pokerHand = tieingPokerHands[i];
                        HandsTiedCount[pokerHand.HoleCardsNumericRepresentation][pokerHand.HandRank / 100_000_000_000L]++;
                    }
                }
                else
                {
                    HandsWonCount[holeCardsNumeric][strongestHand.HandRank / 100_000_000_000L]++;
                }
            }
        }

        private void ShuffleDeck()
        {
            //Put all the cards back in the deck.
            var cardIndexPosition = Deck.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
            }

            //Shuffle the deck.
            cardIndexPosition = Deck.Length;
            while (cardIndexPosition > 1)
            {
                cardIndexPosition--;
                int cardPositionToSwap = NumberGenerator.Next(Deck.Length);
                var swapCard = Deck[cardPositionToSwap];
                Deck[cardPositionToSwap] = Deck[cardIndexPosition];
                Deck[cardIndexPosition] = swapCard;
            }
        }
    }
}
