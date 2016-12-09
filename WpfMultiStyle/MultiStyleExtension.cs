﻿using System;
using System.Windows;
using System.Windows.Markup;

namespace WpfMultiStyle
{
    /// <summary>
    /// 实现一个标记扩展，该标记扩展支持根据 XAML 制作的多个静态（XAML 加载时）<see cref="System.Windows.Style"/> 资源引用。
    /// </summary>
    [MarkupExtensionReturnType(typeof(Style))]
    public class MultiStyleExtension : MarkupExtension
    {
        private string[] _resourceKeys = new string[0];

        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="resourceKeys">多个<see cref="System.Windows.Style"/>资源字典多个 Key</param>
        public MultiStyleExtension(string resourceKeys)
        {
            if (resourceKeys != null && resourceKeys.Length > 0)
            {
                _resourceKeys = resourceKeys.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// 返回一个应在此扩展应用的属性上设置的对象。对于 <see cref="WpfMultiStyle.MultiStyleExtension"/>，这是在资源字典中查找的多个 <see cref="System.Windows.Style"/> 对象，并合并这些对象，其中要查找的对象由 <see cref="System.Windows.StaticResourceExtension.ResourceKey"/> 标识。
        /// </summary>
        /// <param name="serviceProvider">可以为标记扩展提供服务的对象。</param>
        /// <returns>要在计算标记扩展提供的值的属性上设置的对象值。</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Style resultStyle = new Style();

            foreach (string currentResourceKey in _resourceKeys)
            {
                Style currentStyle = new StaticResourceExtension(currentResourceKey).ProvideValue(serviceProvider) as Style;

                /*if (currentStyle == null)
                {
                    throw new InvalidOperationException("找不到 " + currentResourceKey + "标识的资源。");
                }*/

                // 忽略无效的 Style
                if (currentStyle != null)
                {
                    resultStyle.Merge(currentStyle);
                }
            }
            return resultStyle;
        }
    }
}
