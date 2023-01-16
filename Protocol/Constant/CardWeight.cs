using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Constant
{
    /// <summary>
    /// 卡牌权值
    /// </summary>
    public class CardWeight
    {
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int FIVE = 5;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;
        public const int NINE = 9;
        public const int TEN = 10;

        public const int JACK = 11;
        public const int QUEEN = 12;
        public const int KING = 13;

        public const int ONE = 14;
        public const int TWO = 15;

        public const int SJOKER = 16;
        public const int LJOKER = 17;

        public static string GetString(int weight)
        {
            switch (weight)
            {
                case 3:
                    return "3";
                case 4:
                    return "4";
                case 5:
                    return "5";
                case 6:
                    return "6";
                case 7:
                    return "7";
                case 8:
                    return "8";
                case 9:
                    return "9";
                case 10:
                    return "10";
                case 11:
                    return "11";
                case 12:
                    return "12";
                case 13:
                    return "13";
                case 14:
                    return "14";
                case 15:
                    return "15";
                case 16:
                    return "16";
                case 17:
                    return "17";
                default:
                    throw new Exception("不存在这样的权值");
            }
        }

        /// <summary>
        /// 获取卡牌的权值
        /// </summary>
        /// <param name="cardList">选中的卡牌</param>
        /// <param name="cardType">出牌类型</param>
        /// <returns></returns>
        public static int GetWeight(List<CardDto> cardList, int cardType)
        {
            int totalWeight = 0;
            if (cardType == CardType.THREE_ONE || cardType == CardType.THREE_TWO)
            {
                //如果是 三带一 或者说 三带二
                // 3335 4443 5333 3335 3353
                for (int i = 0; i < cardList.Count - 2; i++)
                {
                    if (cardList[i].Weight == cardList[i + 1].Weight && cardList[i].Weight == cardList[i + 2].Weight)
                    {
                        totalWeight += (cardList[i].Weight * 3);
                    }
                }
            }
            else //if(cardType == CardType.STRAIGHT || cardType == CardType.DOUBLE_STRAIGHT)
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    totalWeight += cardList[i].Weight;
                }
            }
            return totalWeight;
        }
    }
}
