﻿using EnvDTE;
using System;

namespace Happy.Scaffolding.MVC.Models
{
    [Serializable]
    public class MetaColumnInfo
    {
        public string ShortTypeName { get; set; }
        public string strDateType { get; set; }
        public euColumnType DataType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Nullable { get; set; }
        public int MaxLength { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }

        public bool IsVisible { get; set; }

        public bool IsNumeric
        {
            get { return ((this.DataType & euColumnType.intCT) == euColumnType.intCT); }
        }

        public bool HasMetaAttribute
        {
            get
            {
                return (MetaAttribute != string.Empty);
            }
        }

        public string MetaAttribute
        {
            get
            {
                switch (this.DataType)
                {
                    case euColumnType.stringCT:
                        if (this.MaxLength > 0)
                            return string.Format("[MaxLength({0})]", this.MaxLength);
                        else
                            break;
                    case euColumnType.intCT:
                    case euColumnType.decimalCT:
                        if (this.RangeMin > 0 || this.RangeMax > 0)
                            return string.Format("[Range({0}, {1})]", this.RangeMin, this.RangeMax);
                        else
                            break;
                    default:
                            break;
                }
                return string.Empty;
            }
        }


        public MetaColumnInfo() { }

        public MetaColumnInfo(Microsoft.AspNet.Scaffolding.Core.Metadata.PropertyMetadata property)
            : this(property.PropertyName, property.ShortTypeName, (property.RelatedModel!=null) )
        {
        }

        public MetaColumnInfo(CodeParameter property)
            : this(property.Name, property.Type.AsString, false)
        {
        }

        public MetaColumnInfo(CodeProperty property)
            : this(property.Name, property.Type.AsString, false)
        {
        }

        private MetaColumnInfo(string strName, string strType, bool relatedModel)
        {
            this.Name = strName;
            this.ShortTypeName = strType;
            this.strDateType = strType.Replace("?", "").Replace("System.", "").ToLower();

            if (!relatedModel)
                this.DataType = GetColumnType(this.strDateType);
            else
            {
                this.DataType = euColumnType.RelatedModel;
            }

            DisplayName = this.Name;
            Nullable = true;
            IsVisible = true;
        }

        private euColumnType GetColumnType(string shortTypeName)
        {
            return ParseEnum<euColumnType>(shortTypeName);
        }

        private static T ParseEnum<T>(string value)
        {
            value = value+ "CT";
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
