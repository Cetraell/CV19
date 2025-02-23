﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace CV19.Infrastructure.Converters
{
    [MarkupExtensionReturnType(typeof(MultiConverter))]
    internal abstract class MultiConverter : IMultiValueConverter
    {
        public abstract object Convert(object[] vv, Type t, object p, CultureInfo c);

        public virtual object[] ConvertBack(object v, Type[] tt, object p, CultureInfo c) =>
            throw new NotSupportedException("Обратное преобразование не поддерживается !");
    }
}
