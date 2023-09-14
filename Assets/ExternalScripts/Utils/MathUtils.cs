using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.ExternalScripts.Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// Найти значение на прямой построенной по определенным двум точкам, исходя из значения на другой прямой с соответсвующими точками
        /// Например: Если 10 метров это 0, а 20 метров это 1, чему равно 15 метров (ответ: 0.5)
        /// </summary>
        /// <param name="value">Искомое значение на первой прямой</param>
        /// <param name="valueP1">Начальная точка первой прямой</param>
        /// <param name="valueP2">Конечная точка первой прямой</param>
        /// <param name="findP1">Начальная точка второй прямой</param>
        /// <param name="findP2">Конечная точка второй прямой</param>
        /// <param name="clamped">Обрезать ли значение по конечным значениям</param>
        /// <returns>Искомое значение на второй прямой</returns>
        public static float Interpolate01(float value, float valueP1, float valueP2, bool clamped)
        {
            return Interpolate(value, valueP1, valueP2, 0, 1, clamped);
        }

        /// <summary>
        /// Найти значение на прямой построенной по определенным двум точкам, исходя из значения на другой прямой с соответсвующими точками
        /// Например: Если 10 метров это 0, а 20 метров это 1, чему равно 15 метров (ответ: 0.5)
        /// </summary>
        /// <param name="value">Искомое значение на первой прямой</param>
        /// <param name="valueP1">Начальная точка первой прямой</param>
        /// <param name="valueP2">Конечная точка первой прямой</param>
        /// <param name="findP1">Начальная точка второй прямой</param>
        /// <param name="findP2">Конечная точка второй прямой</param>
        /// <param name="clamped">Обрезать ли значение по конечным значениям</param>
        /// <returns>Искомое значение на второй прямой</returns>
        public static float Interpolate(float value, float valueP1, float valueP2, float findP1, float findP2, bool clamped)
        {
            double a = ((double)value - valueP1) / ((double)valueP2 - valueP1);
            if (Double.IsNaN(a))
            {
                Debug.LogError("Interpolate Is NaN");
                a = 0;
            }

            if (clamped)
                a = a < 0 ? 0 : a > 1 ? 1 : a;
            return (float)(((double)findP2 - findP1) * a + findP1);
        }

        /// <summary>
        /// Найти значение на прямой построенной по определенным двум точкам, исходя из значения на другой прямой с соответсвующими точками
        /// Например: Если 10 метров это 0, а 20 метров это 1, чему равно 15 метров (ответ: 0.5)
        /// </summary>
        /// <param name="easing">Функция Изинга</param>
        /// <param name="value">Искомое значение на первой прямой</param>
        /// <param name="valueP1">Начальная точка первой прямой</param>
        /// <param name="valueP2">Конечная точка первой прямой</param>
        /// <param name="findP1">Начальная точка второй прямой</param>
        /// <param name="findP2">Конечная точка второй прямой</param>
        /// <param name="clamped">Обрезать ли значение по конечным значениям</param>
        /// <returns>Искомое значение на второй прямой</returns>
        public static float Interpolate(Easing.EasingDelegate easing, float value, float valueP1, float valueP2,
            float findP1, float findP2, bool clamped)
        {
            var a = Interpolate(value, valueP1, valueP2, 0, 1, clamped);

            return (findP2 - findP1) * easing(a) + findP1;
        }

        /// <summary>
        /// Найти значение на прямой построенной по определенным двум точкам, исходя из значения на другой прямой с соответсвующими точками
        /// Например: Если 10 метров это 0, а 20 метров это 1, чему равно 15 метров (ответ: 0.5)
        /// </summary>
        /// <param name="easeType">Тип Изинга</param>
        /// <param name="value">Искомое значение на первой прямой</param>
        /// <param name="valueP1">Начальная точка первой прямой</param>
        /// <param name="valueP2">Конечная точка первой прямой</param>
        /// <param name="findP1">Начальная точка второй прямой</param>
        /// <param name="findP2">Конечная точка второй прямой</param>
        /// <param name="clamped">Обрезать ли значение по конечным значениям</param>
        /// <returns>Искомое значение на второй прямой</returns>
        public static float Interpolate(EaseType easeType, float value, float valueP1, float valueP2,
            float findP1, float findP2, bool clamped)
        {
            return Interpolate(Easing.GetEasingDelegate(easeType), value, valueP1, valueP2, findP1, findP2, clamped);
        }
    }
}
