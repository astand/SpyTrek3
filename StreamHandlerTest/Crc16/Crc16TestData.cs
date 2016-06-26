﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    public static class Crc16TestData
    {

        public static Byte LongArrayHi => 158;

        public static Byte LongArrayLow => 172;

        public static Byte[] LongCleanArray => new Byte[]
        {
0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x03, 0x00, 0x1c, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

        public static Byte ShortArrayHi => 231;

        public static Byte ShortArrayLow => 117;

        public static Byte[] ShortCleanArray => new Byte[] {
0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x06, 0x00, 0x00, 0x00,
0x66, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x33, 0x34, 0x31, 0x37, 0x33, 0x00,
0x62, 0x6c, 0x6b, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x31, 0x32, 0x30, 0x30,
0x00
        };


        public static Byte[] ShortPacked => new byte[]
        {
0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x06, 0x00, 0x00, 0x00,
0x66, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x33, 0x34, 0x31, 0x37, 0x33, 0x00,
0x62, 0x6c, 0x6b, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x31, 0x32, 0x30, 0x30,
0x00, 231, 117
        };


        public static Byte[] LongPacked => new Byte[]
        {
0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x03, 0x00, 0x1c, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 158, 172
            };

        public static Byte[] ShortFullPacked => new byte[]
{
0x00, 0x00, 0x00, 0x00, 0xc0,
0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x06, 0x00, 0x00, 0x00,
0x66, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x33, 0x34, 0x31, 0x37, 0x33, 0x00,
0x62, 0x6c, 0x6b, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x31, 0x32, 0x30, 0x30,
0x00, 231, 117, 0xc0, 0x00

};
        public static Byte[] MixStuffed => new byte[]
        {
0xff, 0x22, 0x41,
0xC0, 0x00, 0x00, 0x80, 0xc3,
0xC0,
0xc0,
0xc0, 0x00, 0xff, 0xfe, 0x12,
0xc0, 0xde, 0x29,
0xc0, 0x00, 0x00, 0x00, 0x00,
0xc0, 0x81, 0x00, 0xc8, 0x6e, 0xc8, 0x00, 0xf0, 0xfe, 0x06, 0x00, 0x00, 0x00,
0x66, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x33, 0x34, 0x31, 0x37, 0x33, 0x00,
0x62, 0x6c, 0x6b, 0x73, 0x69, 0x7a, 0x65, 0x00, 0x31, 0x32, 0x30, 0x30,
0x00, 231, 117,

0xc0, 0x00,
0xc0, 0x85, 0x06, 0xcf, 0x6e, 0x00, 0xcf, 0x1a, 0xf4, 0x00,
0x00, 0x02, 0xd0, 0x8a, 0x7f, 0x6f, 0x94, 0xcc, 0x8c, 0x15,
0xd1, 0xf3, 0xe6, 0x5e, 0x4d, 0x95, 0x08, 0xe5, 0x7d, 0x69,
0x52, 0x45, 0x2c, 0x54, 0x2e, 0xad, 0x3e, 0x20, 0xe9, 0xb8,
0x2c, 0x33, 0x29, 0x68, 0x97, 0x2e, 0x2a, 0x86, 0x8e, 0xd1,
0x07, 0x50, 0x1a, 0x0b, 0x00, 0xaa, 0xca, 0xd8, 0x07, 0xc3,
0x8c, 0x61, 0x02, 0x44, 0x6c, 0x0c, 0x85, 0x3c, 0xca, 0x19,
0xa0, 0xc4, 0x76, 0xa4, 0x5b, 0x57, 0x31, 0x57, 0x36, 0xc2,
0xb0, 0x30, 0x1e, 0xef, 0xd8, 0xfa, 0x09, 0x86, 0xfa, 0xe4,
0xb4, 0xab, 0x6d, 0xdd, 0x07, 0x8f, 0x23, 0x18, 0x0d, 0xd1,
0x58, 0x05, 0x7a, 0x57, 0x15, 0x7b, 0x97, 0xab, 0x52, 0x7e,
0xb9, 0x30, 0x9f, 0x39, 0x37, 0x8f, 0xcf, 0x83, 0xb4, 0x6d,
0x4d, 0xd3, 0xa2, 0xb8, 0x52, 0xc6, 0xee, 0x74, 0x9f, 0xe1,
0x6f, 0x2e, 0xa3, 0x6c, 0x5e, 0xbd, 0x2b, 0x0b, 0xa9, 0xfd,
0x35, 0xdb, 0xdc, 0x2d, 0x75, 0xf4, 0xff, 0x89, 0x8e, 0xfb,
0x3f, 0xd2, 0x41, 0xbc, 0x73, 0x20, 0x9b, 0xe7, 0xef, 0x47,
0xd2, 0xdb, 0xdc, 0x5c, 0x18, 0x00, 0xd2, 0xba, 0x04, 0xbc,
0xb2, 0xdb, 0xdd, 0x0d, 0xe8, 0x25, 0xe6, 0x42, 0x05, 0xd0,
0x13, 0xe2, 0x6d, 0xce, 0xaf, 0x2e, 0xe0, 0x27, 0xf6, 0x39,
0x1d, 0x1c, 0x31, 0xe3, 0xe0, 0x3c, 0x72, 0x8a, 0x05, 0x68,
0xdf, 0x85, 0x5d, 0x64, 0xd4, 0xd5, 0x1d, 0xcf, 0x40, 0x10,
0xc8, 0xdb, 0xdc, 0x3e, 0x34, 0x8e, 0x2d, 0xdf, 0x9e, 0x91,
0x23, 0xb0, 0x3d, 0xe1, 0x41, 0x30, 0x42, 0x9d, 0xb1, 0x1a,
0xf4, 0x11, 0x6e, 0xd9, 0x64, 0xfa, 0xf0, 0x1b, 0x00, 0x00,
0xff, 0xff, 0x03, 0x00, 0x50, 0x4b, 0x03, 0x04, 0x14, 0x00,
0x06, 0x00, 0x08, 0x00, 0x00, 0x00, 0x21, 0x8f, 0x56, 0xc0



        };
    }
}
