$(function ()
{

    $(document).on("touchmove", function (event)
    {
        event.preventDefault();
    })
    $("html,.dialog,.start-btn").tap(function ()
    {
        return false;
    })

    //获取url参数的函数
    function GetQueryString(name)
    {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }

    var lx = GetQueryString("lx");
    var xl; //获取用户选择的路线
    if (lx == 1)
    {
        xl = [{
            txt: '北区路入口',
            fx: 'left',
            type: 'type1'
        }, {
            txt: '世贸中心',
            fx: 'right',
            type: 'type2'
        }, {
            txt: '合景聚融',
            fx: 'left',
            type: 'type2'
        }, {
            txt: '英利国际',
            fx: 'left',
            type: 'type2'
        }, {
            txt: '解放东路出口',
            fx: 'right',
            type: 'type1'
        }];
    } else if (lx == 2)
    {
        xl = [{
            txt: '储奇门入口',
            fx: 'left',
            type: 'type1'
        }, {
            txt: '融创北区',
            fx: 'left',
            type: 'type2'
        }, {
            txt: '解放东路出口',
            fx: 'right',
            type: 'type1'
        }];
    } else
    {
        return false;
    }



    var game = {
        xl: xl,
        speed: 20,
        scale: 0.1,
        offset: 0,
        offsetv: 0,
        end: 1600,
        left: 0,
        num: 0,
        total: 1,
        list: null,
        time: 30,
        timer: null,
        btn: true,
        ran: null,
        istart: false,
        runcount: 0,
        speedjs: 0,
        spobj: null,
        init: function ()
        {
            var _this = this;
            this.btn = $(".start-btn.btn2");
            this.btn1 = $(".start-btn.btn1");
            this.bg3 = $(".bg3");
            this.xt = $(".xt");
            this.xs = $(".xs");
            this.ran = Math.floor(Math.random() * (this.xl.length - 2)) + 1;
            this.bg3.text(this.xl[0].txt);  //显示入口名称
            $(".start-btn").on("touchstart", function ()
            {
                $(this).css({
                    "background": "url('/jfb/images/start1.png')",
                    "background-size": "100% 100%"
                });
            })
            $(".win-btn").tap(function ()
            {
                $("#fx-dialog").show();
            })
            $(".start-btn").on("touchend", function ()
            {
                $(this).css({
                    "background": "url('/jfb/images/start.png')",
                    "background-size": "100% 100%"
                });
            })
            $(".games-btn.true-btn").click(function ()
            {
                $(this).parents(".dialog").hide();
                _this.timer = setInterval(function ()
                {
                    _this.time--;
                    $(".time").text(_this.time + "s");
                    if (_this.time <= 0)
                    {
                        $("#time-dialog").show();
                        _this.btn = false;
                        clearInterval(_this.timer);
                        clearInterval(_this.spobj);
                    }
                }, 1000)
            })
            $(".games-btn.false-btn").click(function ()
            {
                $(this).parents(".dialog").hide();
                $("#sorry-dialog").show();
            })
            this.btn1.tap(function ()
            {
                $(this).hide();
                _this.bg3.hide();
                _this.setrun();
                _this.timer = setInterval(function ()
                {
                    _this.time--;
                    $(".time").text(_this.time + "s");
                    if (_this.time <= 0)
                    {
                        $("#time-dialog").show();
                        _this.btn = false;
                        clearInterval(_this.timer);
                        clearInterval(_this.spobj);
                    }
                }, 1000)
            })
            this.btn.tap(function ()
            {
                if (_this.btn == false)
                {
                    return false;
                }
                _this.beginrun();
            })

            $(".sorry-btn.btn1").click(function ()
            {
                $(this).parents(".dialog").hide();
                $(".time").text("30s");
                _this.rest();
            })
        },
        setrun: function ()
        {
            var _this = this;
            //console.info("run");
            this.spobj = setInterval(function ()
            {
                //console.info("inter: " + _this.runcount);
                if (_this.runcount > 0)
                {
                    _this.runcount--;
                    _this.speedjs += 2;
                    //console.info("js:"+_this.speedjs);
                    _this.run(_this.speedjs);
                }
                else
                {
                    if (_this.speedjs > 0) _this.speedjs -= 2;
                }
            }, 300);
        },
        beginrun: function ()
        {
            this.runcount++;
            this.offsetv += this.speed;
            if (this.offsetv >= 1000) { this.offsetv = 0; }
            this.setlp();
            //console.info("count:"+this.runcount);
        },
        run: function (sp)
        {
            var _this = this;
            this.offset += this.speed + sp;

            this.scale += 0.05;
            this.left += 2;
            if (this.scale >= 1.5)
            {
                this.scale = 1.5;
            }

            if (this.offset >= 1200)
            {
                this.xt.css({
                    "-webkit-transition": "none"
                })
                this.xs.css({
                    "-webkit-transition": "none"
                })
                this.offset = 0;
                this.scale = 0.1;
                this.left = 0;
            }
            else
            {
                this.xt.css({
                    "-webkit-transition": "transform 300ms linear"
                })
                this.xs.css({
                    "-webkit-transition": "transform 300ms linear"
                })
            }
            this.xt.css({
                "-webkit-transform": "translateY(" + _this.offset + "px)"
            })
            this.xs.css({
                "-webkit-transform": "translateY(" + _this.offset + "px) translateX(" + _this.left + "px) scale(" + _this.scale + ")"
            })

        },
        setlp: function ()
        {
            var _this = this;
            this.num = this.end / (this.xl.length - 2);
            if (_this.offsetv % _this.num == 0)
            {
                if (_this.total == _this.xl.length - 1)
                {
                    _this.bg3.show().text(_this.xl[_this.xl.length - 1].txt); //显示出口
                    clearInterval(_this.timer);
                    _this.btn = false;
                    if (_this.time > 0)
                    {
                        setTimeout(function ()
                        {
                            //$("#win-dialog").show();
                            clearInterval(_this.spobj);
                            getredpack();
                        }, 1000);
                    }

                    _this.total = 1;
                } else
                {
                    if (_this.total == _this.ran)
                    {
                        $("#ques-dialog").show();
                        clearInterval(_this.timer);
                    }
                    _this.list = new lp(_this.xl[_this.total]);
                    setTimeout(function ()
                    {
                        _this.list.lp.remove();
                        _this.list = null;
                    }, 2000);
                    _this.total++;
                }

            }
        },
        rest: function ()
        {
            var _this = this;
            this.time = 30;
            this.offset = 0;
            this.scale = 0.1;
            this.num = 0;
            this.total = 1;
            this.left = 0;
            this.btn = true;
            this.btn1.show();
            this.bg3.show().text(this.xl[0].txt);  //显示入口名称
            this.xt.css({
                "-webkit-transition": "none",
                "-webkit-transform": "translateY(" + _this.offset + "px)"
            })
            this.xs.css({
                "-webkit-transition": "none",
                "-webkit-transform": "translateY(" + _this.offset + "px) translateX(" + _this.left + "px) scale(" + _this.scale + ")"
            })
        }
    }
    game.init();

    //路牌类

    function lp(obj)
    {
        this.txt = obj.txt;
        this.fx = obj.fx;
        this.type = obj.type;
        this.lp;
        this.txtCon;
        this.init();
    }
    lp.prototype.init = function ()
    {
        this.creadE();
    }
    lp.prototype.creadE = function ()
    {
        var _this = this;
        if (this.type == "type1")
        {
            this.lp = $('<div class="lp">');
        } else
        {
            this.lp = $('<div class="lp1">')
        }
        if (this.fx == "left")
        {
            this.lp.addClass('left');
        } else
        {
            this.lp.addClass("right");
        }
        this.txtCon = $('<p class="lp-text">' + this.txt + '</p>');
        this.lp.append(this.txtCon);
        $("body").append(this.lp);
    }

    //new lp(xl[0]); 创建路牌的方法

})
