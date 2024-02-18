/* create: pengyingh 210521
Copyright (C) 2017, pengyingh. (http://www.cnblogs.com/pengyingh/)
	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:
	* Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.
	* Redistributions in binary form must reproduce the above
	copyright notice, this list of conditions and the following
	disclaimer in the documentation and/or other materials provided
	with the distribution.
	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
	"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
	LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
	A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
		LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
		DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
	THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
	OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace PModule {
    public class PAutoId
    {
        private uint m_IdOri;
        private uint m_IdMax;
        private uint m_Id;
        public uint AutoId => m_Id < m_IdMax ? m_Id ++ : m_Id = m_IdOri;//自增id，结束时根据id遍历结束

        public PAutoId(uint idOri = 1, uint idMax = 4294967294)
        {
            m_IdOri = idOri;
            m_Id = idOri;
            m_IdMax = idMax + 1;
        }

        public void Reset()
        {
            m_Id = m_IdOri;
        }

        public void Reset(uint idOri, uint idMax = 4294967294)
        {
            m_IdOri = idOri;
            m_Id = idOri;
            m_IdMax = idMax + 1;
        }
    }
}