using System.ComponentModel;
using System.Windows.Forms;

namespace ZeroBase
{
    /// <summary>
    /// 为输入数字专用的TextBox。
    /// </summary>
    /// 
    public enum NumberTypes //定义一个枚举类型
    {
        全部 = 0,
        正数 = 1,
        负数 = 2

    };
    // public enum YesMark{.,-};//定义一个枚举，规定可以输入的字符
    public class NumberBox : System.Windows.Forms.TextBox
    {
        //  private string rString1;
        //  //该正则表达式描述负号，只能出现在最前面
        //  private string rString2=@"[0-9*]";
        //  //该正则表达式描述整形值
        //  private string rString3; //该正则表达式描述浮点值
        public NumberBox(IContainer container)
            : base() //构造函数
        {
            this._maxValue = long.MaxValue;
            this._minValue = long.MinValue;
            this._precision = 3;
            this._numberType = NumberTypes.全部;
        }


        #region "属性"

        private uint _precision;
        ///<summary>
        ///当允许输入浮点数，描述允许的精度
        ///<summary>
        [System.ComponentModel.Browsable(true), System.ComponentModel.DefaultValue(0), System.ComponentModel.Description("当允许输入浮点数后，描述允许的精度")]
        public uint DecimalDigits
        {
            get
            {
                return _precision;
            }
            set
            {
                _precision = value;
                //    rString3=@".0-9{0,"+value.ToString()+"}";
            }

        }

        private long _maxValue;
        ///<summary>
        ///允许输入的上限
        ///</summary>
        [System.ComponentModel.Browsable(true), System.ComponentModel.DefaultValue(0), System.ComponentModel.Description("允许输入的上限")]
        public long MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;

            }

        }

        private long _minValue;
        ///<summary>
        ///允许输入的下限
        ///</summary>
        [System.ComponentModel.Browsable(true), System.ComponentModel.DefaultValue(0), System.ComponentModel.Description("允许输入的下限")]
        public long MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;

            }

        }
        private NumberTypes _numberType;
        ///<summary>
        ///允许输入的数值类型
        ///</summary>
        [System.ComponentModel.Browsable(true), System.ComponentModel.DefaultValue(0), System.ComponentModel.Description("允许输入的数值类型")]
        public NumberTypes NumberType
        {
            get
            {
                return _numberType;
            }
            set
            {
                _numberType = value;
                //    if(value==NumberTypes.负数)
                //    {
                //     rString1=@"[-]";
                //    }
                //    else
                //    {
                //     rString1=@"";
                //    }

            }

        }
        #endregion

        #region "重写方法"

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {

            this.Text = this.Text.Trim();

            if (!char.IsLetter(e.KeyChar) && ((int)e.KeyChar == (int)System.Windows.Forms.Keys.Back || e.KeyChar.ToString() == "." || e.KeyChar.ToString() == "-" || e.KeyChar.ToString() == "0" || e.KeyChar.ToString() == "1" || e.KeyChar.ToString() == "2" || e.KeyChar.ToString() == "3" || e.KeyChar.ToString() == "4" || e.KeyChar.ToString() == "5" || e.KeyChar.ToString() == "6" || e.KeyChar.ToString() == "7" || e.KeyChar.ToString() == "8" || e.KeyChar.ToString() == "9"))//当输入的是字母时不予接收
            {
                if (this._numberType == NumberTypes.全部)
                {
                    //


                }
                if (this._numberType == NumberTypes.正数)
                {    //当为正数时不允许出现负号
                    if (e.KeyChar == '-')
                    {

                        e.Handled = true;
                    }
                }
                if (this._numberType == NumberTypes.负数 || this._numberType == NumberTypes.全部)
                {

                    if (e.KeyChar == '-')
                    {
                        //当输入负号时，光标所在位置不在最前面，则自动移到最前面加负号
                        if (this.SelectionLength == this.Text.Trim().Length)//假如选择的长度和文本的长度一致，则是可以删除的
                        {


                        }
                        else
                        {

                            this.Select(0, 0);
                            if ((this.Text.Trim() != "" && this.Text.Trim().IndexOf("-") >= 0))
                            {
                                e.Handled = true;//负号只能输入一次

                            }

                        }
                    }
                    //如果已经有了负号，并且精度不0则不能在负号后加点号
                    if (this._precision > 0 && this.Text.Trim().IndexOf("-") != -1)
                    {
                        if (e.KeyChar == '.' && this.SelectionStart <= 1)
                        {
                            e.Handled = true;


                        }

                    }
                    //假如已经有了负号，则在负号前面加任何符号和数字都不行
                    if (this.Text.Trim().IndexOf("-") != -1)
                    {
                        if (this.SelectionStart == 0 && this.SelectionLength == 0)
                        {
                            e.Handled = true;

                        }
                    }

                }

                if (char.IsPunctuation(e.KeyChar) && (e.KeyChar != '-'))
                //对标点符号的处理
                {
                    if (this.Text.Trim() == "" || this.Text.Trim() == "-" || this._precision == 0)
                    //第一位不允许出现小数点
                    {

                        e.Handled = true;
                    }
                    else
                    {
                        if (this.Text.IndexOf(".") != -1)
                        //小数点只可以出现一次
                        {
                            e.Handled = true;
                        }
                    }
                }
                if (this.Text.IndexOf(".") != -1 && char.IsNumber(e.KeyChar))
                //处理精度，因为indexof(".")可以描述精度，所以不需要判断_precision
                {
                    if (this.Text.Length - this.Text.IndexOf(".") - 1 == this._precision)
                    {
                        e.Handled = true;
                    }
                }
            }
            else //是字母时不能接收
            {
                e.Handled = true;

            }

            base.OnKeyPress(e);
            //基类处理基本的KeyChar

        }


        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            this.Text = this.Text.Trim();
            if (this.Text.Trim() == "" || this.Text.Trim() == "-")
            {
                this.Text = "0";
            }

            //构造正则表达式对象
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"^([+-]?)\d*\.?\d+$");
            if (r.IsMatch(this.Text) == false)
            {
                e.Cancel = true;
                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "请输入正确的数据！";
                string caption = "数据类型不正确";
                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            }
            else
            {

                //假如输入是零，则直接转换为零
                if (System.Convert.ToDecimal(this.Text.Trim()) == 0)
                {
                    this.Text = "0";
                }
                //假如最后一位是小数点，则自动给予去掉
                if (this.Text.Trim().Length == this.Text.Trim().IndexOf(".") + 1)
                {
                    this.Text = this.Text.Substring(0, this.Text.Trim().Length - 1);

                }

                if (this._numberType == NumberTypes.负数)
                {

                    //假如输入是零，则直接转换为零
                    if (System.Convert.ToDecimal(this.Text.Trim()) == 0)
                    {
                        this.Text = "0";
                    }
                    else if (this.Text.Trim().IndexOf("-") == -1 || this.Text.Trim() == "")
                    {
                        //     this.Focus();
                        //     e.Cancel=true;
                        this.Select(0, 0);//光标定位到第一
                        this.Text = "-" + this.Text.Trim();//在前面加负号
                        //     string message = "请输入正确的数据！";
                        //     string caption = "数据类型不正确";
                        //     System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                        //     MessageBox.Show(this, message, caption, buttons,MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                    }

                }
                //判断用户输入的数值是否介于最大和最小之间
                if (System.Convert.ToDecimal(this.Text.Trim()) < this._minValue || System.Convert.ToDecimal(this.Text.Trim()) > this._maxValue)
                {
                    e.Cancel = true;
                    string message = "请输入规定范围内的数据！";
                    string caption = "数据大小不正确";
                    System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OK;
                    MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                }
                base.OnValidating(e);

            }
        }
        #endregion

    }
}