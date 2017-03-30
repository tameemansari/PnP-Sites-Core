﻿using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.Serializers
{
    /// <summary>
    /// Class to serialize/deserialize the content types
    /// </summary>
    [TemplateSchemaSerializer(SerializationSequence = 400, DeserializationSequence = 400,
        MinimalSupportedSchemaVersion = XMLPnPSchemaVersion.V201605,
        Default = true)]
    internal class ComposedLookSerializer : PnPBaseSchemaSerializer<ComposedLook>
    {
        public override void Deserialize(object persistence, ProvisioningTemplate template)
        {
            var composedLook = persistence.GetPublicInstancePropertyValue("ComposedLook");
            PnPObjectsMapper.MapProperties(composedLook, template.ComposedLook, null, true);
        }

        public override void Serialize(ProvisioningTemplate template, object persistence)
        {
            if ((template.ComposedLook != null)&&(
                template.ComposedLook.BackgroundFile != null ||
                template.ComposedLook.ColorFile != null ||
                template.ComposedLook.FontFile != null ||
                template.ComposedLook.Name != null ||
                template.ComposedLook.Version != 0))
            {
                var composedLookType = Type.GetType($"{PnPSerializationScope.Current?.BaseSchemaNamespace}.ComposedLook, {PnPSerializationScope.Current?.BaseSchemaAssemblyName}", true);
                var target = Activator.CreateInstance(composedLookType, true);
                var expressions = new Dictionary<string, IResolver>();
                expressions.Add($"{composedLookType}.VersionSpecified", new ExpressionValueResolver((s, p) => true));


                PnPObjectsMapper.MapProperties(template.ComposedLook, target, expressions, recursive: true);

                persistence.GetPublicInstanceProperty("ComposedLook").SetValue(persistence, target);
            }
        }
    }
}
