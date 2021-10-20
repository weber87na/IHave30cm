using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHave30cm
{
    public partial class FormRuler : Form
    {
        Font midFont;
        public FormRuler()
        {
            InitializeComponent( );
            //this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle( ControlStyles.ResizeRedraw, true );
            CreateContextMenu( this );

            midFont = new Font( Font.FontFamily, Font.Size / 2 );
        }

        //建立離開選單
        private void CreateContextMenu(Control control)
        {
            ContextMenu contextMenu = new ContextMenu( );
            contextMenu.MenuItems.Add( new MenuItem( "Exit", new EventHandler( Exit) ) );
            control.ContextMenu = contextMenu;
        }
        private void Exit(object sender , EventArgs e)
        {
            Environment.Exit( 0 );
        }
        protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        {
            //繪製邊框
            Graphics g = e.Graphics;
            DrawBorder( g );

            //先轉換為 mm
            //Millimeter = mm
            //1 cm = 10mm
            //驗證工具
            //https://www.ginifab.com.tw/tools/measurement/online_real_size_scale_ruler.html
            g.PageUnit = GraphicsUnit.Pixel;

            Pen p = new Pen( Brushes.Black );
            p.Width = 0.5f;

            //橫的
            g.DrawLine(p, 0, 0, 100.Unit(), 0 );


            //刻度
            for(int i = 1; i <= 300 ; i ++)
            {
                if( i % 10 == 0)
                {
                    //每 100px 畫最大格
                    g.DrawLine( p,
                        i.Unit( ), 0,
                        i.Unit( ), 1.Unit( ) * 2);

                    //取得文字大小
                    var textSize = g.MeasureString( i.Unit().ToString( ), Font );
                    //繪製文字
                    g.DrawString( i.Unit( ).ToString( ), Font, Brushes.Black,
                        i.Unit( ) - textSize.Width / 2, 1.Unit( ) * 2
                        );
                }
                else if ( i % 5 == 0){
                    //每 50px 畫中格
                    g.DrawLine( p,
                        i.Unit( ), 0,
                        i.Unit( ), 1.Unit( )  );

                    //取得文字大小
                    var textSize = g.MeasureString( i.Unit().ToString( ), midFont );
                    //繪製文字
                    g.DrawString( i.Unit( ).ToString( ), midFont, Brushes.Black,
                        i.Unit( ) - textSize.Width / 2, 1.Unit( ) * 2
                        );
                }
                else
                {
                    //正常狀態
                    g.DrawLine( p,
                        i.Unit( ), 0,
                        i.Unit( ), 1.Unit( ) / 2 );
                }

            }
        }


        private void DrawBorder(Graphics g) { 
            g.FillRectangle( Brushes.Black, Top );
            g.FillRectangle( Brushes.Black, Left );
            g.FillRectangle( Brushes.Black, Right );
            g.FillRectangle( Brushes.Black, Bottom );
        }

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 1; // you can rename this variable if you like

        Rectangle Top { get { return new Rectangle( 0, 0, this.ClientSize.Width, _ ); } }
        Rectangle Left { get { return new Rectangle( 0, 0, _, this.ClientSize.Height ); } }
        Rectangle Bottom { get { return new Rectangle( 0, this.ClientSize.Height - _, this.ClientSize.Width, _ ); } }
        Rectangle Right { get { return new Rectangle( this.ClientSize.Width - _, 0, _, this.ClientSize.Height ); } }

        Rectangle TopLeft { get { return new Rectangle( 0, 0, _, _ ); } }
        Rectangle TopRight { get { return new Rectangle( this.ClientSize.Width - _, 0, _, _ ); } }
        Rectangle BottomLeft { get { return new Rectangle( 0, this.ClientSize.Height - _, _, _ ); } }
        Rectangle BottomRight { get { return new Rectangle( this.ClientSize.Width - _, this.ClientSize.Height - _, _, _ ); } }


        //參考怎麼讓沒有 border 的 form 可以進行拖拉跟移動
        //https://stackoverflow.com/questions/2575216/how-to-move-and-resize-a-form-without-a-border
        protected override void WndProc(ref Message message)
        {
            base.WndProc( ref message );

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient( Cursor.Position );

                Point pos = new Point( message.LParam.ToInt32( ) );
                pos = this.PointToClient( pos );
                if (pos.Y < 32)
                {
                    message.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }

                if (TopLeft.Contains( cursor )) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains( cursor )) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains( cursor )) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains( cursor )) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains( cursor )) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains( cursor )) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains( cursor )) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains( cursor )) message.Result = (IntPtr)HTBOTTOM;
            }
        }
    }
}
