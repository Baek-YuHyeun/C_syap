using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp5  //Client
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
        }

        Socket _Sock;

        private void mnuStart_Click(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(tbInterval.Text);
            timer1.Enabled = true; //타이머 시작
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Close();
        }
        int pCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //일정 시간 간격으로 프로토콜 보내줌
            string s1 = tbCode.Text;    // 10001  :5자리
            string s2 = tbModel.Text;   // 안쓰는 데이터 -> 칸은 잡아놓고 데이터 값은 0000 으로 해둠(확장성을 위해) :5자리
            string s3 = tbTemp.Text;    // 10.5  :5자리
            string s4 = tbHum.Text;     // 30.00  :5자리  -> 5자리 이상으로 해야함, 그 이하(3자리 등)로 하면 큰일남 
            string s5 = tbWind.Text;    // 05.70  :5자리
            string s6 = dateTimePicker1.Text;   // 2020년10월....  -> 잘 사용하지 않는 데이터(주로 받는쪽에서 설정)
            //프로토콜 정의 : (통신, 패킷 시작) [STX:02].. (02는 시작하는 통신 헥사 코드)
            //                              [s1:05][s2:05][s3:05][s4:05][s5:05] 
            //                               //(data - 사이즈별로 정의돼있음, 보통 데이터의 자릿수로 잡아놓음)
            //                               ..[ETX:03] (통신, 패킷 끝)
            //프로토콜 정의: [STX:02][s1:05][s2:05][s3:05][s4:05][s5:05][ETX:03] 
            string sPro = $"[STX]{s1}{s2}{s3}{s5}{s6}[ETX]";//대괄호와 자릿수도 뺌(가능하면 패킷은 짧게, 길면 잡음이 들어가기 쉬움)
            _Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //인수는 모두 상수, 거의 정해져있는 코드
            _Sock.Connect(tbServerIP.Text, int.Parse(tbServerPort.Text)); //서버가 호스트
            _Sock.Send(Encoding.Default.GetBytes(sPro)); //통신은 1바이트 문자열(데이터) -> 스트링(프로토콜)을 바이트로 변경 필요(엔코딩 과정 필요)
            tbProtocol.Text = $"[{pCount++}] {sPro}";
        }
    }
}
