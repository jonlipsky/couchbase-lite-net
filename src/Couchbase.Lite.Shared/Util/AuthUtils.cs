﻿//
// AuthUtils.cs
//
// Author:
//     Pasin Suriyentrakorn  <pasin@couchbase.com>
//
// Copyright (c) 2014 Couchbase Inc
// Copyright (c) 2014 .NET Foundation
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// Copyright (c) 2014 Couchbase, Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions
// and limitations under the License.
//

using System;
using System.Net.Http.Headers;
using Couchbase.Lite.Auth;

namespace Couchbase.Lite.Util
{
    internal static class AuthUtils
    {
        const String Tag = "AuthUtils";

        internal static AuthenticationHeaderValue GetAuthenticationHeaderValue(IAuthenticator authenticator, Uri uri)
        {
            AuthenticationHeaderValue authHeader = null;

            var userInfo = uri != null ? uri.UserInfo : null;
            if (!String.IsNullOrEmpty(userInfo)) 
            {
                authHeader = uri.GetAuthenticationHeader("Basic");
                if (authHeader == null)
                {
                    Log.W(Tag, "Unable to parse user info, not setting credentials");
                }
            } 
            else 
            {
                if (authenticator != null) 
                {
                    userInfo = authenticator.UserInfo;
                    var scheme = authenticator.Scheme;
                    if (userInfo != null && scheme != null)
                    {
                        authHeader = userInfo.AsAuthenticationHeader(scheme);
                    }
                }
            }

            return authHeader;
        }
    }
}

