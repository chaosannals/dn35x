using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dn35x.Base
{
    public static class TextExtends
    {
        public static readonly Regex SnakePattern = new Regex(@"([ _-]+|[A-Z]+)");
        public static readonly Regex KebabPattern = new Regex(@"([ _-]+|[A-Z]+)");
        public static readonly Regex PascalPattern = new Regex(@"([ _-]?[A-Za-z][0-9A-Za-z]*)");

        /// <summary>
        /// 转蛇皮风格。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSnake(this string text)
        {
            return SnakePattern.Replace(text, t =>
            {
                if (t.Value.All(a => " _-".Contains(a))) return "_";
                return "_" + t.Value.ToLower();
            }).Trim('_');
        }

        /// <summary>
        /// 转烤串风格。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToKebab(this string text)
        {
            return KebabPattern.Replace(text, t =>
            {
                if (t.Value.All(a => " _-".Contains(a))) return "-";
                return "-" + t.Value.ToLower();
            }).Trim('-');
        }

        /// <summary>
        /// 转帕斯卡风格。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascal(this string text)
        {
            return PascalPattern.Replace(text, t =>
            {
                return t.Value.Trim(' ', '_', '-').ToFirstUpper();
            });
        }

        /// <summary>
        /// 首字符大写。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFirstUpper(this string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }
    }
}
