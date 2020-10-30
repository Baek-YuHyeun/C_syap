using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp4 //Server
{
    public partial class frmServer : Form
    {
        public frmServer()
        {
            InitializeComponent();
        }

        delegate void AddTextCallBack(string str); //로컬함수 아님
                                                   //delegate: 함수의 포인터 -> 함수의 주소만 참조해서 호출
        private void AddText(string str) 
        {
            if (this.tbMemo.InvokeRequired) 
            {
                AddTextCallBack d = new AddTextCallBack(AddText);
                this.Invoke(d, new object[] { str });
            }
            else 
            {
                tbMemo.Text += str;
            }

        }

        public string GetToken(int n, string str, string sep = ",") //구분자 디폴트값: "," => 이런 선택적 매개변수는 괄호안 인수 중 가장 뒤에 와야함
        {

            //임시 변수 선언
            int i, j, k; //local index
            int n1 = 0, n2 = 0, n3 = 0; //temp int  변수
            string sRet; //return value
            //0kim, 1lee 
            //인수 n 도 제로베이스, n = 0 이면 첫번째 문자열(0kim) 출력

            //모든 인덱스의 필드를 찾을 수 있게 for 문 사용(일반화)
            for (i = 0; i < n; i++)
            {
                n1 = str.IndexOf(sep, n1) + 1;
                if (n1 == 0) return ""; //인수 n 값이 문자열 개수 초과
            }

            n2 = str.IndexOf(sep, n1); //n = 0 일 경우
            if (n2 == -1) //n 이 마지막 문자열을 출력할 경우
            {
                n2 = str.Length;
            }

            n3 = n2 - n1;
            sRet = str.Substring(n1, n3); // 인덱스(제로베이스 0), 사이즈(제로베이스 x)
            return sRet;

        }

        TcpListener _Listen;
        TcpClient _Sock;
        byte[] buf = new byte[200000]; //바이트 버퍼, 버퍼 사이즈는 최대한 크게 잡아놓는게 좋음

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(_Listen == null) //버튼을 여러번 클릭하는 사용자도 있을 가능성 고려
                _Listen = new TcpListener(int.Parse(tbPort.Text));  //TcpListener(port IP(주소))
            _Listen.Start(); //시스템이나 통신 관련 변수는 앞에 _(언더라인) 붙여서 변수명 지음

            Thread ServerThread = new Thread(ServerProcess);
            ServerThread.Start();


            /*//이 부분을 쓰레드로 만들어줘야함(따로 돌아가는 별도의 프로그램)
            //외부로부터의 접속을 받아서 처리
            _Sock = _Listen.AcceptTcpClient(); //세션 오픈 (서버가 클라이언트에게 소켓을 되돌려줌)
                                              //외부로부터의 리퀘스트를 받을때까지 멈춰있음 (쓰레드로 분리 필요)
            NetworkStream ns = _Sock.GetStream(); // 받은 데이터(리퀘스트)를 읽음, 데이터가 들어왔는지도 확인 가능
            if (ns.DataAvailable) 
            {
                ns.Read(buf, 0, (int)(ns.Length)); //바이트 버퍼 필요, buf: byte array
                tbMemo.Text += buf.ToString() + "\r\n";

            }*/

           // _Listen.Stop();
            
            //여기까지가 한 개의 세션에 대한 프로세스
           
            
            
        }

        private void ServerProcess() // 서버 Thread  함수
        {
            while (true) 
            {
                 //외부로부터의 접속을 받아서 처리
                _Sock = _Listen.AcceptTcpClient(); //세션 오픈 (서버가 클라이언트에게 소켓을 되돌려줌)
                                                   //외부로부터의 리퀘스트를 받을때까지 멈춰있음 (쓰레드로 분리 필요)


                //tbMemo.Text += _Sock.Client.RemoteEndPoint.ToString(); //에러: 서로 연결 안됨 -> 인버트와 델리게이트 과정 필요
                //Cross Thread error: tbMemo 에 직접 접근 불가 => 인보크 필요                                                    
                string s1 = GetToken(0, _Sock.Client.RemoteEndPoint.ToString(), ":");
                AddText($"원격 접속 요청 : {s1}\r\n");
                
                NetworkStream ns = _Sock.GetStream(); // 받은 데이터(리퀘스트)를 읽음, 데이터가 들어왔는지도 확인 가능
                if (ns.DataAvailable)
                {
                    ns.Read(buf, 0, (int)(buf.Length)); //바이트 버퍼 필요, buf: byte array
                    //tbMemo.Text += buf.ToString() + "\r\n"; //Cross Thread error
                    AddText(Encoding.Default.GetString(buf) + "\r\n"); //바이트에서 스트링(원래 형태)로 변환, 복원(엔코딩 필요)

                }
            }//계속 외부의 접속(리퀘스트)을 기다리고 있음
           
        }
    }
}
