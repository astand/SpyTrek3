﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandlerTests.TrekWriter
{
    public static class TrekDescriptorArray
    {
        public static byte[] Array = {
            0x0b, 0x00, 0x0f, 0x0a, 0x11, 0x0d, 0x1f, 0x07, 0x0f, 0x0a, 0x11, 0x0e, 0x01, 0x19, 0xd8, 0x09,
            0x00, 0x00, 0x21, 0xa0, 0x04, 0x00, 0x13, 0xfa, 0x4b, 0x00, 0x0c, 0x00, 0x0f, 0x0a, 0x11, 0x0f,
            0x01, 0x33, 0x0f, 0x0a, 0x11, 0x0f, 0x18, 0x00, 0x00, 0x07, 0x00, 0x00, 0xe8, 0x1f, 0x01, 0x00,
            0x0c, 0x1a, 0x4d, 0x00, 0x0d, 0x00, 0x0f, 0x0a, 0x11, 0x11, 0x2b, 0x02, 0x0f, 0x0a, 0x11, 0x12,
            0x0a, 0x29, 0xa8, 0x07, 0x00, 0x00, 0x8f, 0x8d, 0x01, 0x00, 0xdf, 0xac, 0x4e, 0x00, 0x0e, 0x00,
            0x0f, 0x0a, 0x12, 0x07, 0x29, 0x03, 0x0f, 0x0a, 0x12, 0x07, 0x36, 0x00, 0x04, 0x06, 0x00, 0x00,
            0x7c, 0xdc, 0x00, 0x00, 0x7e, 0x8a, 0x4f, 0x00, 0x0f, 0x00, 0x0f, 0x0a, 0x12, 0x08, 0x1c, 0x31,
            0x0f, 0x0a, 0x12, 0x08, 0x25, 0x20, 0x9c, 0x03, 0x00, 0x00, 0x4c, 0x96, 0x00, 0x00, 0xe0, 0x20,
            0x50, 0x00, 0x10, 0x00, 0x0f, 0x0a, 0x12, 0x08, 0x37, 0x32, 0x0f, 0x0a, 0x12, 0x09, 0x0f, 0x31,
            0xe4, 0x06, 0x00, 0x00, 0x21, 0x13, 0x01, 0x00, 0x1a, 0x34, 0x51, 0x00, 0x11, 0x00, 0x0f, 0x0a,
            0x12, 0x09, 0x2b, 0x03, 0x0f, 0x0a, 0x12, 0x09, 0x2e, 0x1f, 0x50, 0x01, 0x00, 0x00, 0x8b, 0x20,
            0x00, 0x00, 0xcb, 0x54, 0x51, 0x00, 0x12, 0x00, 0x0f, 0x0a, 0x12, 0x0a, 0x2e, 0x1d, 0x0f, 0x0a,
            0x12, 0x0a, 0x30, 0x31, 0x18, 0x01, 0x00, 0x00, 0xe8, 0x1e, 0x00, 0x00, 0xdf, 0x73, 0x51, 0x00,
            0x13, 0x00, 0x0f, 0x0a, 0x12, 0x0b, 0x00, 0x2c, 0x0f, 0x0a, 0x12, 0x0b, 0x07, 0x34, 0x10, 0x03,
            0x00, 0x00, 0x3d, 0x7c, 0x00, 0x00, 0x42, 0xf0, 0x51, 0x00, 0x14, 0x00, 0x0f, 0x0a, 0x12, 0x0b,
            0x30, 0x1f, 0x0f, 0x0a, 0x12, 0x0b, 0x39, 0x05, 0xb4, 0x04, 0x00, 0x00, 0xf0, 0xa3, 0x00, 0x00,
            0x41, 0x94, 0x52, 0x00, 0x15, 0x00, 0x0f, 0x0a, 0x12, 0x10, 0x02, 0x03, 0x0f, 0x0a, 0x12, 0x11,
            0x1a, 0x08, 0x08, 0x1a, 0x00, 0x00, 0x27, 0x60, 0x0c, 0x00, 0x7c, 0xf4, 0x5e, 0x00, 0x16, 0x00,
            0x0f, 0x0a, 0x12, 0x14, 0x18, 0x14, 0x0f, 0x0a, 0x12, 0x15, 0x22, 0x09, 0x70, 0x15, 0x00, 0x00,
            0x6a, 0x64, 0x0c, 0x00, 0xfb, 0x58, 0x6b, 0x00, 0x17, 0x00, 0x0f, 0x0a, 0x13, 0x06, 0x1f, 0x22,
            0x0f, 0x0a, 0x13, 0x06, 0x28, 0x07, 0xd4, 0x03, 0x00, 0x00, 0x92, 0x75, 0x00, 0x00, 0xa6, 0xce,
            0x6b, 0x00, 0x18, 0x00, 0x0f, 0x0a, 0x13, 0x06, 0x32, 0x1e, 0x0f, 0x0a, 0x13, 0x07, 0x10, 0x13,
            0xd8, 0x09, 0x00, 0x00, 0x5c, 0x99, 0x01, 0x00, 0x19, 0x68, 0x6d, 0x00, 0x19, 0x00, 0x0f, 0x0a,
            0x13, 0x07, 0x25, 0x1c, 0x0f, 0x0a, 0x13, 0x07, 0x32, 0x39, 0xf4, 0x02, 0x00, 0x00, 0xa9, 0x49,
            0x00, 0x00, 0xd8, 0xb1, 0x6d, 0x00, 0x1a, 0x00, 0x0f, 0x0a, 0x13, 0x08, 0x0b, 0x27, 0x0f, 0x0a,
            0x13, 0x08, 0x0e, 0x3b, 0xa4, 0x01, 0x00, 0x00, 0x5f, 0x2b, 0x00, 0x00, 0x51, 0xdd, 0x6d, 0x00,
            0x1b, 0x00, 0x0f, 0x0a, 0x13, 0x08, 0x34, 0x27, 0x0f, 0x0a, 0x13, 0x08, 0x3b, 0x2e, 0x64, 0x03,
            0x00, 0x00, 0x2d, 0x76, 0x00, 0x00, 0xe1, 0x53, 0x6e, 0x00, 0x1c, 0x00, 0x0f, 0x0a, 0x13, 0x0c,
            0x1d, 0x2e, 0x0f, 0x0a, 0x13, 0x0c, 0x26, 0x0a, 0x64, 0x03, 0x00, 0x00, 0x61, 0x5d, 0x00, 0x00,
            0x4c, 0xb1, 0x6e, 0x00, 0x1d, 0x00, 0x0f, 0x0a, 0x13, 0x0d, 0x23, 0x0e, 0x0f, 0x0a, 0x13, 0x0d,
            0x29, 0x2a, 0x84, 0x02, 0x00, 0x00, 0x90, 0x46, 0x00, 0x00, 0xeb, 0xf7, 0x6e, 0x00, 0x1e, 0x00,
            0x0f, 0x0a, 0x13, 0x0f, 0x19, 0x06, 0x0f, 0x0a, 0x13, 0x0f, 0x27, 0x0a, 0xcc, 0x05, 0x00, 0x00,
            0x27, 0xdf, 0x00, 0x00, 0x34, 0xd7, 0x6f, 0x00, 0x1f, 0x00, 0x0f, 0x0a, 0x14, 0x07, 0x05, 0x09,
            0x0f, 0x0a, 0x14, 0x07, 0x0f, 0x32, 0x60, 0x04, 0x00, 0x00, 0xfc, 0xa1, 0x00, 0x00, 0x4b, 0x79,
            0x70, 0x00, 0x20, 0x00, 0x0f, 0x0a, 0x14, 0x07, 0x1e, 0x34, 0x0f, 0x0a, 0x14, 0x07, 0x27, 0x07,
            0xf0, 0x03, 0x00, 0x00, 0x5a, 0x94, 0x00, 0x00, 0xda, 0x0d, 0x71, 0x00, 0x21, 0x00, 0x0f, 0x0a,
            0x14, 0x07, 0x3a, 0x3b, 0x0f, 0x0a, 0x14, 0x08, 0x1e, 0x08, 0xa0, 0x09, 0x00, 0x00, 0x54, 0xe6,
            0x01, 0x00, 0x47, 0xf4, 0x72, 0x00, 0x22, 0x00, 0x0f, 0x0a, 0x14, 0x08, 0x29, 0x09, 0x0f, 0x0a,
            0x14, 0x08, 0x2e, 0x22, 0xa0, 0x02, 0x00, 0x00, 0x53, 0xb1, 0x00, 0x00, 0xac, 0xa5, 0x73, 0x00,
            0x23, 0x00, 0x0f, 0x0a, 0x14, 0x09, 0x34, 0x0f, 0x0f, 0x0a, 0x14, 0x0a, 0x0f, 0x09, 0x04, 0x06,
            0x00, 0x00, 0x56, 0xe9, 0x00, 0x00, 0xc9, 0x8f, 0x74, 0x00};
    }
}
