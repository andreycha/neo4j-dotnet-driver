﻿// Copyright (c) 2002-2018 "Neo Technology,"
// Network Engine for Objects in Lund AB [http://neotechnology.com]
// 
// This file is part of Neo4j.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Neo4j.Driver.V1;

namespace Neo4j.Driver.Internal.IO.StructHandlers
{
    internal class TimeHandler : IPackStreamStructHandler
    {
        public const byte StructType = (byte) 't';
        public const int StructSize = 1;

        public IEnumerable<byte> ReadableStructs => new[] {StructType};

        public IEnumerable<Type> WritableTypes => new[] {typeof(CypherTime)};

        public object Read(IPackStreamReader reader, byte signature, long size)
        {
            PackStream.EnsureStructSize("LocalTime", StructSize, size);

            var nanosOfDay = reader.ReadLong();

            return TemporalHelpers.NanoOfDayToTime(nanosOfDay);
        }

        public void Write(IPackStreamWriter writer, object value)
        {
            var time = value.CastOrThrow<CypherTime>();

            writer.WriteStructHeader(StructSize, StructType);
            writer.Write(time.ToNanoOfDay());
        }
    }
}