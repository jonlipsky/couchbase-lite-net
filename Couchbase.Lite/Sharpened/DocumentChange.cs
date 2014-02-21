//
// DocumentChange.cs
//
// Author:
//	Zachary Gramana  <zack@xamarin.com>
//
// Copyright (c) 2013, 2014 Xamarin Inc (http://www.xamarin.com)
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
/**
* Original iOS version by Jens Alfke
* Ported to Android by Marty Schoch, Traun Leyden
*
* Copyright (c) 2012, 2013, 2014 Couchbase, Inc. All rights reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
* except in compliance with the License. You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software distributed under the
* License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
* either express or implied. See the License for the specific language governing permissions
* and limitations under the License.
*/

using System;
using Couchbase.Lite.Internal;
using Sharpen;

namespace Couchbase.Lite
{
	public class DocumentChange
	{
		[InterfaceAudience.Private]
		internal DocumentChange(RevisionInternal addedRevision, RevisionInternal winningRevision
			, bool isConflict, Uri sourceUrl)
		{
			this.addedRevision = addedRevision;
			this.winningRevision = winningRevision;
			this.isConflict = isConflict;
			this.sourceUrl = sourceUrl;
		}

		private RevisionInternal addedRevision;

		private RevisionInternal winningRevision;

		private bool isConflict;

		private Uri sourceUrl;

		public virtual string GetDocumentId()
		{
			return addedRevision.GetDocId();
		}

		public virtual string GetRevisionId()
		{
			return addedRevision.GetRevId();
		}

		public virtual bool IsCurrentRevision()
		{
			return winningRevision != null && addedRevision.GetRevId().Equals(winningRevision
				.GetRevId());
		}

		public virtual bool IsConflict()
		{
			return isConflict;
		}

		public virtual Uri GetSourceUrl()
		{
			return sourceUrl;
		}

		[InterfaceAudience.Private]
		public virtual RevisionInternal GetAddedRevision()
		{
			return addedRevision;
		}

		[InterfaceAudience.Private]
		internal virtual RevisionInternal GetWinningRevision()
		{
			return winningRevision;
		}

		public static Couchbase.Lite.DocumentChange TempFactory(RevisionInternal revisionInternal
			, Uri sourceUrl, bool inConflict)
		{
			Couchbase.Lite.DocumentChange change = new Couchbase.Lite.DocumentChange(revisionInternal
				, null, inConflict, sourceUrl);
			// TODO: fix winning revision here
			return change;
		}
	}
}