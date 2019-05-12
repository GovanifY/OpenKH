﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace kh.kh2.Messages
{
    public partial class MsgSerializer
    {
        private static Dictionary<MessageCommand, Func<MessageCommandModel, SerializeModel, XNode>> _xmlCustomSerializer =
            new Dictionary<MessageCommand, Func<MessageCommandModel, SerializeModel, XNode>>()
            {
                [MessageCommand.PrintText] = (msgCmd, model) => new XElement("text", model.ValueGetter(msgCmd)),
            };

        public static XElement SerializeXEntries(IEnumerable<Msg.Entry> entries, bool ignoreExceptions = false)
        {
            return new XElement("messages", entries.Select(x =>
            {
                List<MessageCommandModel> messageDecoded;

                try
                {
                    messageDecoded = Encoders.InternationalSystem.Decode(x.Data);
                }
                catch (NotImplementedException ex)
                {
                    if (ignoreExceptions)
                        return new XElement("msgerror",
                            new XAttribute("id", x.Id),
                            ex.Message);
                    else
                        throw ex;
                }

                return SerializeXEntries(x.Id, messageDecoded, ignoreExceptions); ;
            }));
        }

        public static XElement SerializeXEntries(int id, IEnumerable<MessageCommandModel> entries, bool ignoreExceptions = false)
        {
            var root = new XElement("message", new XAttribute("id", id));
            foreach (var entry in entries)
            {
                XNode node;

                try
                {
                    if (!_serializer.TryGetValue(entry.Command, out var serializeModel))
                        throw new NotImplementedException($"The command {entry.Command} serialization is not implemented yet.");

                    if (serializeModel == null)
                        break;

                    if (_xmlCustomSerializer.TryGetValue(entry.Command, out var xmlCustomSerializer))
                        node = xmlCustomSerializer(entry, serializeModel);
                    else
                        node = GetDefaultSerializer(entry, serializeModel);
                }
                catch (NotImplementedException ex)
                {
                    if (ignoreExceptions)
                        node = new XElement("error", ex.Message);
                    else
                        throw ex;
                }

                root.Add(node);
            }
            return root;
        }

        private static XNode GetDefaultSerializer(MessageCommandModel msgCmd, SerializeModel model)
        {
            var attribute = new XAttribute("value", model.ValueGetter(msgCmd));
            return attribute != null ? new XElement(model.Name, attribute) : new XElement(model.Name);
        }
    }
}