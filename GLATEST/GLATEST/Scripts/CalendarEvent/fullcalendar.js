(function (o, K) {
    function Ia(a, b) {
        a._id = a._id || (a.id === K ? "_fc" + Eb++ : a.id + "");
        if (a.date) {
            if (!a.start) a.start = a.date;
            delete a.date
        }
        a._start = q(a.start = ib(a.start));
        a.end = ib(a.end);
        if (a.end && a.end <= a.start) a.end = null;
        a._end = a.end ? q(a.end) : null;
        if (a.allDay === K) a.allDay = b.allDayDefault;
        if (a.className) {
            if (typeof a.className == "string") a.className = a.className.split(/\s+/)
        } else a.className = []
    }
    function Wa(a, b, f, c) {
        function g(m, e, j, r) {
            N = m;
            L = e;
            ca = b.theme ? "ui" : "fc";
            O = b.weekends ? 0 : 1;
            ua = b.firstDay;
            if (ra = b.isRTL) {
                ka = -1;
                R = L - 1
            } else {
                ka = 1;
                R = 0
            }
            var s = A.start.getMonth(),
				I = Ga(new Date),
				E, B = q(A.visStart);
            if (U) {
                t();
                e = U.find("tr").length;
                if (N < e) U.find("tr:gt(" + (N - 1) + ")").remove();
                else if (N > e) {
                    m = "";
                    for (e = e; e < N; e++) {
                        m += "<tr class='fc-week" + e + "'>";
                        for (E = 0; E < L; E++) {
                            m += "<td class='fc-" + za[B.getDay()] + " " + ca + "-state-default fc-new fc-day" + (e * L + E) + (E == R ? " fc-leftmost" : "") + "'>" + (r ? "<div class='fc-day-number'></div>" : "") + "<div class='fc-day-content'><div style='position:relative'>&nbsp;</div></div></td>";
                            C(B, 1);
                            O && da(B)
                        }
                        m += "</tr>"
                    }
                    U.append(m)
                }
                fa(U.find("td.fc-new").removeClass("fc-new"));
                B = q(A.visStart);
                U.find("td").each(function () {
                    var W = o(this);
                    if (N > 1) B.getMonth() == s ? W.removeClass("fc-other-month") : W.addClass("fc-other-month"); +B == +I ? W.removeClass("fc-not-today").addClass("fc-today").addClass(ca + "-state-highlight") : W.addClass("fc-not-today").removeClass("fc-today").removeClass(ca + "-state-highlight");
                    W.find("div.fc-day-number").text(B.getDate());
                    C(B, 1);
                    O && da(B)
                });
                if (N == 1) {
                    B = q(A.visStart);
                    x.find("th").each(function () {
                        o(this).text(oa(B, j, b));
                        this.className = this.className.replace(/^fc-\w+(?= )/, "fc-" + za[B.getDay()]);
                        C(B, 1);
                        O && da(B)
                    });
                    B = q(A.visStart);
                    U.find("td").each(function () {
                        this.className = this.className.replace(/^fc-\w+(?= )/, "fc-" + za[B.getDay()]);
                        C(B, 1);
                        O && da(B)
                    })
                }
            } else {
                var J = o("<table/>").appendTo(a);
                m = "<thead><tr>";
                for (e = 0; e < L; e++) {
                    m += "<th class='fc-" + za[B.getDay()] + " " + ca + "-state-default" + (e == R ? " fc-leftmost" : "") + "'>" + oa(B, j, b) + "</th>";
                    C(B, 1);
                    O && da(B)
                }
                x = o(m + "</tr></thead>").appendTo(J);
                m = "<tbody>";
                B = q(A.visStart);
                for (e = 0; e < N; e++) {
                    m += "<tr class='fc-week" + e + "'>";
                    for (E = 0; E < L; E++) {
                        m += "<td class='fc-" + za[B.getDay()] + " " + ca + "-state-default fc-day" + (e * L + E) + (E == R ? " fc-leftmost" : "") + (N > 1 && B.getMonth() != s ? " fc-other-month" : "") + (+B == +I ? " fc-today " + ca + "-state-highlight" : " fc-not-today") + "'>" + (r ? "<div class='fc-day-number'>" + B.getDate() + "</div>" : "") + "<div class='fc-day-content'><div style='position:relative'>&nbsp;</div></div></td>";
                        C(B, 1);
                        O && da(B)
                    }
                    m += "</tr>"
                }
                U = o(m + "</tbody>").appendTo(J);
                fa(U.find("td"));
                P = o("<div style='position:absolute;z-index:8;top:0;left:0'/>").appendTo(a)
            }
        }
        function n(m) {
            Fa = m;
            m = U.find("tr td:first-child");
            var e = Fa - x.height(),
				j;
            if (b.weekMode == "variable") j = e = Math.floor(e / (N == 1 ? 2 : 6));
            else {
                j = Math.floor(e / N);
                e = e - j * (N - 1)
            }
            if (Xa === K) {
                var r = U.find("tr:first").find("td:first");
                r.height(j);
                Xa = j != r.height()
            }
            if (Xa) {
                m.slice(0, -1).height(j);
                m.slice(-1).height(e)
            } else {
                Sa(m.slice(0, -1), j);
                Sa(m.slice(-1), e)
            }
        }
        function k(m) {
            va = m;
            la.clear();
            Ja(x.find("th").slice(0, -1), Aa = Math.floor(va / L))
        }
        function y(m) {
            A.reportEvents(wa = m);
            M(H(m))
        }
        function w(m) {
            t();
            M(H(wa), m)
        }
        function t() {
            A._clearEvents();
            P.empty()
        }
        function H(m) {
            var e = q(A.visStart),
				j = C(q(e), L),
				r = o.map(m, Pa),
				s, I, E, B, J, W, Ba = [];
            for (s = 0; s < N; s++) {
                I = Ya(A.sliceSegs(m, r, e, j));
                for (E = 0; E < I.length; E++) {
                    B = I[E];
                    for (J = 0; J < B.length; J++) {
                        W = B[J];
                        W.row = s;
                        W.level = E;
                        Ba.push(W)
                    }
                }
                C(e, 7);
                C(j, 7)
            }
            return Ba
        }
        function M(m, e) {
            jb(m, N, A, 0, va, function (j) {
                return U.find("tr:eq(" + j + ")")
            }, la.left, la.right, P, sa, e)
        }
        function sa(m, e, j) {
            A.eventElementHandlers(m, e);
            if (m.editable || m.editable === K && b.editable) {
                G(m, e);
                j.isEnd && A.resizableDayEvent(m, e, Aa)
            }
        }
        function G(m, e) {
            if (!b.disableDragging && e.draggable) {
                var j;
                e.draggable({
                    zIndex: 9,
                    delay: 50,
                    opacity: A.option("dragOpacity"),
                    revertDuration: b.dragRevertDuration,
                    start: function (r, s) {
                        A.trigger("eventDragStart", e, m, r, s);
                        A.hideEvents(m, e);
                        Z.start(function (I, E, B, J) {
                            e.draggable("option", "revert", !I || !B && !J);
                            ga();
                            if (I) {
                                j = B * 7 + J * ka;
                                ma(C(q(m.start), j), C(Pa(m), j))
                            } else j = 0
                        }, r, "drag")
                    },
                    stop: function (r, s) {
                        Z.stop();
                        ga();
                        A.trigger("eventDragStop", e, m, r, s);
                        if (j) {
                            e.find("a").removeAttr("href");
                            A.eventDrop(this, m, j, 0, m.allDay, r, s)
                        } else {
                            o.browser.msie && e.css("filter", "");
                            A.showEvents(m, e)
                        }
                    }
                })
            }
        }
        function fa(m) {
            m.click(D).mousedown(ia)
        }
        function D(m) {
            if (!A.option("selectable")) {
                var e = parseInt(this.className.match(/fc\-day(\d+)/)[1]);
                e = C(q(A.visStart), Math.floor(e / L) * 7 + e % L);
                A.trigger("dayClick", this, e, true, m)
            }
        }
        function Q(m, e, j, r) {
            $ = true;
            A.trigger("select", A, m, e, j, r)
        }
        function ha(m) {
            if ($) {
                ga();
                $ = false;
                A.trigger("unselect", A, m)
            }
        }
        function ma(m, e) {
            for (var j = q(A.visStart), r = C(q(j), L), s = 0; s < N; s++) {
                var I = new Date(Math.max(j, m)),
					E = new Date(Math.min(r, e));
                if (I < E) {
                    var B;
                    if (ra) {
                        B = Ca(E, j) * ka + R + 1;
                        I = Ca(I, j) * ka + R + 1
                    } else {
                        B = Ca(I, j);
                        I = Ca(E, j)
                    }
                    fa(pa(s, B, s, I - 1))
                }
                C(j, 7);
                C(r, 7)
            }
        }
        function pa(m, e, j, r) {
            m = aa.rect(m, e, j, r, a);
            return A.renderOverlay(m, a)
        }
        function ga() {
            A.clearOverlays()
        }
        function X(m) {
            return C(q(A.visStart), m.row * 7 + m.col * ka + R)
        }
        var ca, ua, O, ra, ka, R, va, Fa, N, L, Aa, x, U, wa = [],
			P, la = new kb(function (m) {
			    return U.find("td:eq(" + (m - Math.max(ua, O) + L) % L + ") div div")
			}),
			A = o.extend(this, lb, f, {
			    renderGrid: g,
			    renderEvents: y,
			    rerenderEvents: w,
			    clearEvents: t,
			    setHeight: n,
			    setWidth: k,
			    defaultEventEnd: function (m) {
			        return q(m.start)
			    }
			});
        A.name = c;
        A.init(a, b);
        mb(a.addClass("fc-grid"));
        var aa = new nb(function (m, e) {
            var j, r, s, I = U.find("tr:first td");
            if (ra) I = o(I.get().reverse());
            I.each(function (E, B) {
                j = o(B);
                r = j.offset().left;
                if (E) s[1] = r;
                s = [r];
                e[E] = s
            });
            s[1] = r + j.outerWidth();
            U.find("tr").each(function (E, B) {
                j = o(B);
                r = j.offset().top;
                if (E) s[1] = r;
                s = [r];
                m[E] = s
            });
            s[1] = r + j.outerHeight()
        }),
			Z = new ob(aa),
			$ = false,
			ia = pb(A, Z, X, function () {
			    return true
			}, ma, ga, Q, ha);
        A.select = function (m, e, j) {
            aa.build();
            ha();
            e || (e = q(m));
            ma(m, C(q(e), 1));
            Q(m, e, j)
        };
        A.unselect = ha;
        qb(A, ha);
        A.dragStart = function (m, e) {
            Z.start(function (j) {
                ga();
                j && pa(j.row, j.col, j.row, j.col)
            }, e)
        };
        A.dragStop = function (m, e, j) {
            var r = Z.stop();
            ga();
            if (r) {
                r = X(r);
                A.trigger("drop", m, r, true, e, j)
            }
        }
    }
    function jb(a, b, f, c, g, n, k, y, w, t, H) {
        var M = f.options,
			sa = M.isRTL,
			G, fa = a.length,
			D, Q, ha, ma, pa, ga = "",
			X = {},
			ca = {},
			ua = [],
			O = [];
        for (G = 0; G < fa; G++) {
            D = a[G];
            Q = D.event;
            ha = "fc-event fc-event-hori ";
            if (sa) {
                if (D.isStart) ha += "fc-corner-right ";
                if (D.isEnd) ha += "fc-corner-left ";
                ma = D.isEnd ? k(D.end.getDay() - 1) : c;
                pa = D.isStart ? y(D.start.getDay()) : g
            } else {
                if (D.isStart) ha += "fc-corner-left ";
                if (D.isEnd) ha += "fc-corner-right ";
                ma = D.isStart ? k(D.start.getDay()) : c;
                pa = D.isEnd ? y(D.end.getDay() - 1) : g
            }
            ga += "<div class='" + ha + Q.className.join(" ") + "' style='position:absolute;z-index:8;left:" + ma + "px'><a" + (Q.url ? " href='" + Ka(Q.url) + "'" : "") + ">" + (!Q.allDay && D.isStart ? "<span class='fc-event-time'>" + Ka(Ha(Q.start, Q.end, f.option("timeFormat"), M)) + "</span>" : "") + "<span class='fc-event-title'>" + Ka(Q.title) + "</span></a>" + ((Q.editable || Q.editable === K && M.editable) && !M.disableResizing && o.fn.resizable ? "<div class='ui-resizable-handle ui-resizable-" + (sa ? "w" : "e") + "'></div>" : "") + "</div>";
            D.left = ma;
            D.outerWidth = pa - ma
        }
        w[0].innerHTML = ga;
        g = w.children();
        for (G = 0; G < fa; G++) {
            D = a[G];
            c = o(g[G]);
            Q = D.event;
            k = f.trigger("eventRender", Q, Q, c);
            if (k === false) c.remove();
            else {
                if (k && k !== true) {
                    c.remove();
                    c = o(k).css({
                        position: "absolute",
                        left: D.left
                    }).appendTo(w)
                }
                D.element = c;
                if (Q._id === H) t(Q, c, D);
                else c[0]._fci = G;
                f.reportEventElement(Q, c)
            }
        }
        rb(w, a, t);
        for (G = 0; G < fa; G++) {
            D = a[G];
            if (c = D.element) {
                t = X[w = D.key = sb(c[0])];
                D.hsides = t === K ? (X[w] = Za(c[0], true)) : t
            }
        }
        for (G = 0; G < fa; G++) {
            D = a[G];
            if (c = D.element) c[0].style.width = D.outerWidth - D.hsides + "px"
        }
        for (G = 0; G < fa; G++) {
            D = a[G];
            if (c = D.element) {
                t = ca[w = D.key];
                D.outerHeight = c[0].offsetHeight + (t === K ? (ca[w] = tb(c[0])) : t)
            }
        }
        for (X = G = 0; X < b; X++) {
            for (ca = w = t = 0; G < fa && (D = a[G]).row == X; ) {
                if (D.level != w) {
                    ca += t;
                    t = 0;
                    w++
                }
                t = Math.max(t, D.outerHeight || 0);
                D.top = ca;
                G++
            }
            ua[X] = n(X).find("td:first div.fc-day-content > div").height(ca + t)
        }
        for (X = 0; X < b; X++) O[X] = ua[X][0].offsetTop;
        for (G = 0; G < fa; G++) {
            D = a[G];
            if (c = D.element) {
                c[0].style.top = O[D.row] + D.top + "px";
                Q = D.event;
                f.trigger("eventAfterRender", Q, Q, c)
            }
        }
    }
    function ub(a, b, f, c) {
        function g(d, i) {
            m = d;
            na = b.theme ? "ui" : "fc";
            La = b.weekends ? 0 : 1;
            vb = b.firstDay;
            if (Ta = b.isRTL) {
                Y = -1;
                ta = m - 1
            } else {
                Y = 1;
                ta = 0
            }
            Da = wb(b.minTime);
            Ua = wb(b.maxTime);
            d = Ta ? C(q(v.visEnd), -1) : q(v.visStart);
            var h = q(d),
				p = Ga(new Date);
            if (A) {
                G();
                A.find("tr:first th").slice(1, -1).each(function () {
                    o(this).text(oa(h, i, b));
                    this.className = this.className.replace(/^fc-\w+(?= )/, "fc-" + za[h.getDay()]);
                    C(h, Y);
                    La && da(h, Y)
                });
                h = q(d);
                ia.find("td").each(function () {
                    this.className = this.className.replace(/^fc-\w+(?= )/, "fc-" + za[h.getDay()]); +h == +p ? o(this).removeClass("fc-not-today").addClass("fc-today").addClass(na + "-state-highlight") : o(this).addClass("fc-not-today").removeClass("fc-today").removeClass(na + "-state-highlight");
                    C(h, Y);
                    La && da(h, Y)
                })
            } else {
                var l, u, z = b.slotMinutes % 15 == 0,
					F = "<div class='fc-agenda-head' style='position:relative;z-index:4'><table style='width:100%'><tr class='fc-first" + (b.allDaySlot ? "" : " fc-last") + "'><th class='fc-leftmost " + na + "-state-default'>&nbsp;</th>";
                for (l = 0; l < m; l++) {
                    F += "<th class='fc-" + za[h.getDay()] + " " + na + "-state-default'>" + oa(h, i, b) + "</th>";
                    C(h, Y);
                    La && da(h, Y)
                }
                F += "<th class='" + na + "-state-default'>&nbsp;</th></tr>";
                if (b.allDaySlot) F += "<tr class='fc-all-day'><th class='fc-axis fc-leftmost " + na + "-state-default'>" + b.allDayText + "</th><td colspan='" + m + "' class='" + na + "-state-default'><div class='fc-day-content'><div style='position:relative'>&nbsp;</div></div></td><th class='" + na + "-state-default'>&nbsp;</th></tr><tr class='fc-divider fc-last'><th colspan='" + (m + 2) + "' class='" + na + "-state-default fc-leftmost'><div/></th></tr>";
                F += "</table></div>";
                A = o(F).appendTo(a);
                w(A.find("td"));
                W = o("<div style='position:absolute;z-index:8;top:0;left:0'/>").appendTo(A);
                h = xb();
                var T = ba(q(h), Ua);
                ba(h, Da);
                F = "<table>";
                for (l = 0; h < T; l++) {
                    u = h.getMinutes();
                    F += "<tr class='" + (!l ? "fc-first" : !u ? "" : "fc-minor") + "'><th class='fc-axis fc-leftmost " + na + "-state-default'>" + (!z || !u ? oa(h, b.axisFormat) : "&nbsp;") + "</th><td class='fc-slot" + l + " " + na + "-state-default'><div style='position:relative'>&nbsp;</div></td></tr>";
                    ba(h, b.slotMinutes);
                    e++
                }
                F += "</table>";
                aa = o("<div class='fc-agenda-body' style='position:relative;z-index:2;overflow:auto'/>").append(Z = o("<div style='position:relative;overflow:hidden'>").append($ = o(F))).appendTo(a);
                t(aa.find("td"));
                Ba = o("<div style='position:absolute;z-index:8;top:0;left:0'/>").appendTo(Z);
                h = q(d);
                F = "<div class='fc-agenda-bg' style='position:absolute;z-index:1'><table style='width:100%;height:100%'><tr class='fc-first'>";
                for (l = 0; l < m; l++) {
                    F += "<td class='fc-" + za[h.getDay()] + " " + na + "-state-default " + (!l ? "fc-leftmost " : "") + (+h == +p ? na + "-state-highlight fc-today" : "fc-not-today") + "'><div class='fc-day-content'><div>&nbsp;</div></div></td>";
                    C(h, Y);
                    La && da(h, Y)
                }
                F += "</tr></table></div>";
                ia = o(F).appendTo(a)
            }
        }
        function n() {
            var d = xb(),
				i = q(d);
            i.setHours(b.firstHour);
            var h = O(d, i) + 1;
            d = function () {
                aa.scrollTop(h)
            };
            d();
            setTimeout(d, 0)
        }
        function k(d, i) {
            E = d;
            $a = {};
            aa.height(d - A.height());
            s = aa.find("tr:first div").height() + 1;
            ia.css({
                top: A.find("tr").height(),
                height: d
            });
            i && n()
        }
        function y(d) {
            I = d;
            Qa.clear();
            aa.width(d);
            $.width("");
            d = A.find("tr:first th");
            var i = ia.find("td"),
				h = aa[0].clientWidth;
            $.width(h);
            j = 0;
            Ja(A.find("tr:lt(2) th:first").add(aa.find("tr:first th")).width("").each(function () {
                j = Math.max(j, o(this).outerWidth())
            }), j);
            r = Math.floor((h - j) / m);
            Ja(i.slice(0, -1), r);
            Ja(d.slice(1, -2), r);
            Ja(d.slice(-2, -1), h - j - r * (m - 1));
            ia.css({
                left: j,
                width: h - j
            })
        }
        function w(d) {
            d.click(H).mousedown(Fb)
        }
        function t(d) {
            d.click(H).mousedown(ra)
        }
        function H(d) {
            if (!v.option("selectable")) {
                var i = Math.min(m - 1, Math.floor((d.pageX - ia.offset().left) / r));
                i = C(q(v.visStart), i * Y + ta);
                var h = this.className.match(/fc-slot(\d+)/);
                if (h) {
                    h = parseInt(h[1]) * b.slotMinutes;
                    var p = Math.floor(h / 60);
                    i.setHours(p);
                    i.setMinutes(h % 60 + Da);
                    v.trigger("dayClick", this, i, false, d)
                } else v.trigger("dayClick", this, i, true, d)
            }
        }
        function M(d, i) {
            v.reportEvents(J = d);
            var h, p = d.length,
				l = [],
				u = [];
            for (h = 0; h < p; h++) d[h].allDay ? l.push(d[h]) : u.push(d[h]);
            Q(fa(l), i);
            ha(D(u), i)
        }
        function sa(d) {
            G();
            M(J, d)
        }
        function G() {
            v._clearEvents();
            W.empty();
            Ba.empty()
        }
        function fa(d) {
            d = Ya(v.sliceSegs(d, o.map(d, Pa), v.visStart, v.visEnd));
            var i, h = d.length,
				p, l, u, z = [];
            for (i = 0; i < h; i++) {
                p = d[i];
                for (l = 0; l < p.length; l++) {
                    u = p[l];
                    u.row = 0;
                    u.level = i;
                    z.push(u)
                }
            }
            return z
        }
        function D(d) {
            var i = ba(q(v.visStart), Da),
				h = o.map(d, U),
				p, l, u, z, F, T, S = [];
            for (p = 0; p < m; p++) {
                l = Ya(v.sliceSegs(d, h, i, ba(q(i), Ua - Da)));
                Gb(l);
                for (u = 0; u < l.length; u++) {
                    z = l[u];
                    for (F = 0; F < z.length; F++) {
                        T = z[F];
                        T.col = p;
                        T.level = u;
                        S.push(T)
                    }
                }
                C(i, 1, true)
            }
            return S
        }
        function Q(d, i) {
            if (b.allDaySlot) {
                jb(d, 1, v, j, I, function () {
                    return A.find("tr.fc-all-day")
                }, function (h) {
                    return j + Qa.left(wa(h))
                }, function (h) {
                    return j + Qa.right(wa(h))
                }, W, pa, i);
                k(E)
            }
        }
        function ha(d, i) {
            var h, p = d.length,
				l, u, z, F, T, S, V, ea, ja, xa, ab = "",
				Ma = {},
				yb = {};
            for (h = 0; h < p; h++) {
                l = d[h];
                u = l.event;
                z = "fc-event fc-event-vert ";
                if (l.isStart) z += "fc-corner-top ";
                if (l.isEnd) z += "fc-corner-bottom ";
                F = O(l.start, l.start);
                T = O(l.start, l.end);
                S = l.col;
                V = l.level;
                ea = l.forward || 0;
                ja = j + Qa.left(S * Y + ta);
                xa = j + Qa.right(S * Y + ta) - ja;
                xa = Math.min(xa - 6, xa * 0.95);
                S = V ? xa / (V + ea + 1) : ea ? (xa / (ea + 1) - 6) * 2 : xa;
                V = ja + xa / (V + ea + 1) * V * Y + (Ta ? xa - S : 0);
                l.top = F;
                l.left = V;
                l.outerWidth = S;
                l.outerHeight = T - F;
                ab += ma(u, l, z)
            }
            Ba[0].innerHTML = ab;
            F = Ba.children();
            for (h = 0; h < p; h++) {
                l = d[h];
                u = l.event;
                z = o(F[h]);
                T = v.trigger("eventRender", u, u, z);
                if (T === false) z.remove();
                else {
                    if (T && T !== true) {
                        z.remove();
                        z = o(T).css({
                            position: "absolute",
                            top: l.top,
                            left: l.left
                        }).appendTo(Ba)
                    }
                    l.element = z;
                    if (u._id === i) ga(u, z, l);
                    else z[0]._fci = h;
                    v.reportEventElement(u, z)
                }
            }
            rb(Ba, d, ga);
            for (h = 0; h < p; h++) {
                l = d[h];
                if (z = l.element) {
                    i = Ma[u = l.key = sb(z[0])];
                    l.vsides = i === K ? (Ma[u] = bb(z[0], true)) : i;
                    i = yb[u];
                    l.hsides = i === K ? (yb[u] = Za(z[0], true)) : i;
                    u = z.find("span.fc-event-title");
                    if (u.length) l.titleTop = u[0].offsetTop
                }
            }
            for (h = 0; h < p; h++) {
                l = d[h];
                if (z = l.element) {
                    z[0].style.width = l.outerWidth - l.hsides + "px";
                    z[0].style.height = (Ma = l.outerHeight - l.vsides) + "px";
                    u = l.event;
                    if (l.titleTop !== K && Ma - l.titleTop < 10) {
                        z.find("span.fc-event-time").text(oa(u.start, v.option("timeFormat")) + " - " + u.title);
                        z.find("span.fc-event-title").remove()
                    }
                    v.trigger("eventAfterRender", u, u, z)
                }
            }
        }
        function ma(d, i, h) {
            return "<div class='" + h + d.className.join(" ") + "' style='position:absolute;z-index:8;top:" + i.top + "px;left:" + i.left + "px'><a" + (d.url ? " href='" + Ka(d.url) + "'" : "") + "><span class='fc-event-bg'></span><span class='fc-event-time'>" + Ka(Ha(d.start, d.end, v.option("timeFormat"))) + "</span><span class='fc-event-title'>" + Ka(d.title) + "</span></a>" + ((d.editable || d.editable === K && b.editable) && !b.disableResizing && o.fn.resizable ? "<div class='ui-resizable-handle ui-resizable-s'>=</div>" : "") + "</div>"
        }
        function pa(d, i, h) {
            v.eventElementHandlers(d, i);
            if (d.editable || d.editable === K && b.editable) {
                X(d, i, h.isStart);
                h.isEnd && v.resizableDayEvent(d, i, r)
            }
        }
        function ga(d, i, h) {
            v.eventElementHandlers(d, i);
            if (d.editable || d.editable === K && b.editable) {
                var p = i.find("span.fc-event-time");
                ca(d, i, p);
                h.isEnd && ua(d, i, p)
            }
        }
        function X(d, i, h) {
            if (!b.disableDragging && i.draggable) {
                var p, l = true,
					u;
                i.draggable({
                    zIndex: 9,
                    opacity: v.option("dragOpacity", "month"),
                    revertDuration: b.dragRevertDuration,
                    start: function (F, T) {
                        v.trigger("eventDragStart", i, d, F, T);
                        v.hideEvents(d, i);
                        p = i.width();
                        Ea.start(function (S, V, ea, ja) {
                            i.draggable("option", "revert", !S || !ea && !ja);
                            x();
                            if (S) {
                                u = ja * Y;
                                if (S.row) {
                                    if (h && l) {
                                        Sa(i.width(r - 10), s * Math.round((d.end ? (d.end - d.start) / Hb : b.defaultEventMinutes) / b.slotMinutes));
                                        i.draggable("option", "grid", [r, 1]);
                                        l = false
                                    }
                                } else {
                                    N(C(q(d.start), u), C(Pa(d), u));
                                    z()
                                }
                            }
                        }, F, "drag")
                    },
                    stop: function (F, T) {
                        var S = Ea.stop();
                        x();
                        v.trigger("eventDragStop", i, d, F, T);
                        if (S && (!l || u)) {
                            i.find("a").removeAttr("href");
                            S = 0;
                            l || (S = Math.round((i.offset().top - Z.offset().top) / s) * b.slotMinutes + Da - (d.start.getHours() * 60 + d.start.getMinutes()));
                            v.eventDrop(this, d, u, S, l, F, T)
                        } else {
                            z();
                            o.browser.msie && i.css("filter", "");
                            v.showEvents(d, i)
                        }
                    }
                });

                function z() {
                    if (!l) {
                        i.width(p).height("").draggable("option", "grid", null);
                        l = true
                    }
                }
            }
        }
        function ca(d, i, h) {
            if (!b.disableDragging && i.draggable) {
                var p, l = false,
					u, z, F;
                i.draggable({
                    zIndex: 9,
                    scroll: false,
                    grid: [r, s],
                    axis: m == 1 ? "y" : false,
                    opacity: v.option("dragOpacity"),
                    revertDuration: b.dragRevertDuration,
                    start: function (V, ea) {
                        v.trigger("eventDragStart", i, d, V, ea);
                        v.hideEvents(d, i);
                        o.browser.msie && i.find("span.fc-event-bg").hide();
                        p = i.position();
                        z = F = 0;
                        Ea.start(function (ja, xa, ab, Ma) {
                            i.draggable("option", "revert", !ja);
                            x();
                            if (ja) {
                                u = Ma * Y;
                                if (b.allDaySlot && !ja.row) {
                                    if (!l) {
                                        l = true;
                                        h.hide();
                                        i.draggable("option", "grid", null)
                                    }
                                    N(C(q(d.start), u), C(Pa(d), u))
                                } else S()
                            }
                        }, V, "drag")
                    },
                    drag: function (V, ea) {
                        z = Math.round((ea.position.top - p.top) / s) * b.slotMinutes;
                        if (z != F) {
                            l || T(z);
                            F = z
                        }
                    },
                    stop: function (V, ea) {
                        var ja = Ea.stop();
                        x();
                        v.trigger("eventDragStop", i, d, V, ea);
                        if (ja && (u || z || l)) v.eventDrop(this, d, u, l ? 0 : z, l, V, ea);
                        else {
                            S();
                            i.css(p);
                            T(0);
                            o.browser.msie && i.css("filter", "").find("span.fc-event-bg").css("display", "");
                            v.showEvents(d, i)
                        }
                    }
                });

                function T(V) {
                    var ea = ba(q(d.start), V),
						ja;
                    if (d.end) ja = ba(q(d.end), V);
                    h.text(Ha(ea, ja, v.option("timeFormat")))
                }
                function S() {
                    if (l) {
                        h.css("display", "");
                        i.draggable("option", "grid", [r, s]);
                        l = false
                    }
                }
            }
        }
        function ua(d, i, h) {
            if (!b.disableResizing && i.resizable) {
                var p, l;
                i.resizable({
                    handles: {
                        s: "div.ui-resizable-s"
                    },
                    grid: s,
                    start: function (u, z) {
                        p = l = 0;
                        v.hideEvents(d, i);
                        o.browser.msie && o.browser.version == "6.0" && i.css("overflow", "hidden");
                        i.css("z-index", 9);
                        v.trigger("eventResizeStart", this, d, u, z)
                    },
                    resize: function (u, z) {
                        p = Math.round((Math.max(s, i.height()) - z.originalSize.height) / s);
                        if (p != l) {
                            h.text(Ha(d.start, !p && !d.end ? null : ba(v.eventEnd(d), b.slotMinutes * p), v.option("timeFormat")));
                            l = p
                        }
                    },
                    stop: function (u, z) {
                        v.trigger("eventResizeStop", this, d, u, z);
                        if (p) v.eventResize(this, d, 0, b.slotMinutes * p, u, z);
                        else {
                            i.css("z-index", 8);
                            v.showEvents(d, i)
                        }
                    }
                })
            }
        }
        function O(d, i) {
            d = q(d, true);
            if (i < ba(q(d), Da)) return 0;
            if (i >= ba(q(d), Ua)) return Z.height();
            d = b.slotMinutes;
            i = i.getHours() * 60 + i.getMinutes() - Da;
            var h = Math.floor(i / d),
				p = $a[h];
            if (p === K) p = $a[h] = aa.find("tr:eq(" + h + ") td div")[0].offsetTop;
            return Math.max(0, Math.round(p - 1 + s * (i % d / d)))
        }
        function ra(d) {
            if (v.option("selectable")) {
                R(d);
                var i = this,
					h;
                Ea.start(function (p, l) {
                    Fa();
                    if (p && p.col == l.col && !la(p)) {
                        l = P(l);
                        p = P(p);
                        h = [l, ba(q(l), b.slotMinutes), p, ba(q(p), b.slotMinutes)].sort(zb);
                        va(h[0], h[3])
                    } else h = null
                }, d);
                o(document).one("mouseup", function (p) {
                    Ea.stop();
                    if (h) {
+h[0] == +h[1] && v.trigger("dayClick", i, h[0], false, p);
                        ka(h[0], h[3], false, p)
                    }
                })
            }
        }
        function ka(d, i, h, p) {
            cb = true;
            v.trigger("select", v, d, i, h, p)
        }
        function R(d) {
            if (cb) {
                Fa();
                cb = false;
                v.trigger("unselect", v, d)
            }
        }
        function va(d, i) {
            var h = v.option("selectHelper");
            if (h) {
                var p = Ca(d, v.visStart) * Y + ta;
                if (p >= 0 && p < m) {
                    p = Ra.rect(0, p, 0, p, Z);
                    var l = O(d, d),
						u = O(d, i);
                    if (u > l) {
                        p.top = l;
                        p.height = u - l;
                        p.left += 2;
                        p.width -= 5;
                        if (o.isFunction(h)) {
                            if (d = h(d, i)) {
                                p.position = "absolute";
                                p.zIndex = 8;
                                qa = o(d).css(p).appendTo(Z)
                            }
                        } else {
                            qa = o(ma({
                                title: "",
                                start: d,
                                end: i,
                                className: [],
                                editable: false
                            }, p, "fc-event fc-event-vert fc-corner-top fc-corner-bottom "));
                            o.browser.msie && qa.find("span.fc-event-bg").hide();
                            qa.css("opacity", v.option("dragOpacity"))
                        }
                        if (qa) {
                            t(qa);
                            Z.append(qa);
                            Ja(qa, p.width, true);
                            Sa(qa, p.height, true)
                        }
                    }
                }
            } else Aa(d, i)
        }
        function Fa() {
            x();
            if (qa) {
                qa.remove();
                qa = null
            }
        }
        function N(d, i) {
            var h;
            if (Ta) {
                h = Ca(i, v.visStart) * Y + ta + 1;
                d = Ca(d, v.visStart) * Y + ta + 1
            } else {
                h = Ca(d, v.visStart);
                d = Ca(i, v.visStart)
            }
            h = Math.max(0, h);
            d = Math.min(m, d);
            h < d && w(L(0, h, 0, d - 1))
        }
        function L(d, i, h, p) {
            d = Ra.rect(d, i, h, p, A);
            return v.renderOverlay(d, A)
        }
        function Aa(d, i) {
            for (var h = q(v.visStart), p = C(q(h), 1), l = 0; l < m; l++) {
                var u = new Date(Math.max(h, d)),
					z = new Date(Math.min(p, i));
                if (u < z) {
                    var F = l * Y + ta;
                    F = Ra.rect(0, F, 0, F, Z);
                    u = O(h, u);
                    z = O(h, z);
                    F.top = u;
                    F.height = z - u;
                    t(v.renderOverlay(F, Z))
                }
                C(h, 1);
                C(p, 1)
            }
        }
        function x() {
            v.clearOverlays()
        }
        function U(d) {
            return d.end ? q(d.end) : ba(q(d.start), b.defaultEventMinutes)
        }
        function wa(d) {
            return (d - Math.max(vb, La) + m) % m * Y + ta
        }
        function P(d) {
            var i = C(q(v.visStart), d.col * Y + ta);
            d = d.row;
            b.allDaySlot && d--;
            d >= 0 && ba(i, Da + d * b.slotMinutes);
            return i
        }
        function la(d) {
            return b.allDaySlot && !d.row
        }
        var A, aa, Z, $, ia, m, e = 0,
			j, r, s, I, E, B, J = [],
			W, Ba, na, vb, La, Ta, Y, ta, Da, Ua, Qa = new kb(function (d) {
			    return ia.find("td:eq(" + d + ") div div")
			}),
			$a = {},
			v = o.extend(this, lb, f, {
			    renderAgenda: g,
			    renderEvents: M,
			    rerenderEvents: sa,
			    clearEvents: G,
			    setHeight: k,
			    setWidth: y,
			    beforeHide: function () {
			        B = aa.scrollTop()
			    },
			    afterShow: function () {
			        aa.scrollTop(B)
			    },
			    defaultEventEnd: function (d) {
			        var i = q(d.start);
			        if (d.allDay) return i;
			        return ba(i, b.defaultEventMinutes)
			    }
			});
        v.name = c;
        v.init(a, b);
        mb(a.addClass("fc-agenda"));
        var Ra = new nb(function (d, i) {
            function h(V) {
                return Math.max(F, Math.min(T, V))
            }
            var p, l, u;
            ia.find("td").each(function (V, ea) {
                p = o(ea);
                l = p.offset().left;
                if (V) u[1] = l;
                u = [l];
                i[V] = u
            });
            u[1] = l + p.outerWidth();
            if (b.allDaySlot) {
                p = A.find("td");
                l = p.offset().top;
                d[0] = [l, l + p.outerHeight()]
            }
            for (var z = Z.offset().top, F = aa.offset().top, T = F + aa.outerHeight(), S = 0; S < e; S++) d.push([h(z + s * S), h(z + s * (S + 1))])
        }),
			Ea = new ob(Ra),
			cb = false,
			Fb = pb(v, Ea, P, la, N, x, ka, R);
        v.select = function (d, i, h) {
            Ra.build();
            R();
            if (h) {
                if (b.allDaySlot) {
                    i || (i = q(d));
                    N(d, C(q(i), 1))
                }
            } else {
                i || (i = ba(q(d), b.slotMinutes));
                va(d, i)
            }
            ka(d, i, h)
        };
        v.unselect = R;
        qb(v, R);
        var qa;
        v.dragStart = function (d, i) {
            Ea.start(function (h) {
                x();
                if (h) if (la(h)) L(h.row, h.col, h.row, h.col);
                else {
                    h = P(h);
                    var p = ba(q(h), b.defaultEventMinutes);
                    Aa(h, p)
                }
            }, i)
        };
        v.dragStop = function (d, i, h) {
            var p = Ea.stop();
            x();
            p && v.trigger("drop", d, P(p), la(p), i, h)
        }
    }
    function Gb(a) {
        var b, f, c, g, n, k;
        for (b = a.length - 1; b > 0; b--) {
            g = a[b];
            for (f = 0; f < g.length; f++) {
                n = g[f];
                for (c = 0; c < a[b - 1].length; c++) {
                    k = a[b - 1][c];
                    if (Ab(n, k)) k.forward = Math.max(k.forward || 0, (n.forward || 0) + 1)
                }
            }
        }
    }
    function rb(a, b, f) {
        a.unbind("mouseover").mouseover(function (c) {
            for (var g = c.target, n; g != this; ) {
                n = g;
                g = g.parentNode
            }
            if ((g = n._fci) !== K) {
                n._fci = K;
                n = b[g];
                f(n.event, n.element, n);
                o(c.target).trigger(c)
            }
            c.stopPropagation()
        })
    }
    function Ya(a) {
        var b = [],
			f, c = a.length,
			g, n, k, y;
        for (f = 0; f < c; f++) {
            g = a[f];
            for (n = 0; ; ) {
                k = false;
                if (b[n]) for (y = 0; y < b[n].length; y++) if (Ab(b[n][y], g)) {
                    k = true;
                    break
                }
                if (k) n++;
                else break
            }
            if (b[n]) b[n].push(g);
            else b[n] = [g]
        }
        return b
    }
    function Ib(a, b) {
        return (b.msLength - a.msLength) * 100 + (a.event.start - b.event.start)
    }
    function Ab(a, b) {
        return a.end > b.start && a.start < b.end
    }
    function pb(a, b, f, c, g, n, k, y) {
        return function (w) {
            if (a.option("selectable")) {
                y(w);
                var t = this,
					H;
                b.start(function (M, sa) {
                    n();
                    if (M && c(M)) {
                        H = [f(sa), f(M)].sort(zb);
                        g(H[0], C(q(H[1]), 1), true)
                    } else H = null
                }, w);
                o(document).one("mouseup", function (M) {
                    b.stop();
                    if (H) {
+H[0] == +H[1] && a.trigger("dayClick", t, H[0], true, M);
                        k(H[0], H[1], true, M)
                    }
                })
            }
        }
    }
    function qb(a, b) {
        a.option("selectable") && a.option("unselectAuto") && o(document).mousedown(function (f) {
            var c = a.option("unselectCancel");
            if (c) if (o(f.target).parents(c).length) return;
            b(f)
        })
    }
    function db(a, b, f) {
        a.setFullYear(a.getFullYear() + b);
        f || Ga(a);
        return a
    }
    function eb(a, b, f) {
        if (+a) {
            b = a.getMonth() + b;
            var c = q(a);
            c.setDate(1);
            c.setMonth(b);
            a.setMonth(b);
            for (f || Ga(a); a.getMonth() != c.getMonth(); ) a.setDate(a.getDate() + (a < c ? 1 : -1))
        }
        return a
    }
    function C(a, b, f) {
        if (+a) {
            b = a.getDate() + b;
            var c = q(a);
            c.setHours(9);
            c.setDate(b);
            a.setDate(b);
            f || Ga(a);
            fb(a, c)
        }
        return a
    }
    function fb(a, b) {
        if (+a) for (; a.getDate() != b.getDate(); ) a.setTime(+a + (a < b ? 1 : -1) * Jb)
    }
    function ba(a, b) {
        a.setMinutes(a.getMinutes() + b);
        return a
    }
    function Ga(a) {
        a.setHours(0);
        a.setMinutes(0);
        a.setSeconds(0);
        a.setMilliseconds(0);
        return a
    }
    function q(a, b) {
        if (b) return Ga(new Date(+a));
        return new Date(+a)
    }
    function xb() {
        var a = 0,
			b;
        do b = new Date(1970, a++, 1);
        while (b.getHours());
        return b
    }
    function da(a, b, f) {
        for (b = b || 1; !a.getDay() || f && a.getDay() == 1 || !f && a.getDay() == 6; ) C(a, b);
        return a
    }
    function Ca(a, b) {
        return Math.round((q(a, true) - q(b, true)) / Bb)
    }
    function Cb(a, b, f, c) {
        if (b !== K && b != a.getFullYear()) {
            a.setDate(1);
            a.setMonth(0);
            a.setFullYear(b)
        }
        if (f !== K && f != a.getMonth()) {
            a.setDate(1);
            a.setMonth(f)
        }
        c !== K && a.setDate(c)
    }
    function Ja(a, b, f) {
        a.each(function (c, g) {
            g.style.width = b - Za(g, f) + "px"
        })
    }
    function Sa(a, b, f) {
        a.each(function (c, g) {
            g.style.height = b - bb(g, f) + "px"
        })
    }
    function Za(a, b) {
        return (parseFloat(jQuery.curCSS(a, "paddingLeft", true)) || 0) + (parseFloat(jQuery.curCSS(a, "paddingRight", true)) || 0) + (parseFloat(jQuery.curCSS(a, "borderLeftWidth", true)) || 0) + (parseFloat(jQuery.curCSS(a, "borderRightWidth", true)) || 0) + (b ? Kb(a) : 0)
    }
    function Kb(a) {
        return (parseFloat(jQuery.curCSS(a, "marginLeft", true)) || 0) + (parseFloat(jQuery.curCSS(a, "marginRight", true)) || 0)
    }
    function bb(a, b) {
        return (parseFloat(jQuery.curCSS(a, "paddingTop", true)) || 0) + (parseFloat(jQuery.curCSS(a, "paddingBottom", true)) || 0) + (parseFloat(jQuery.curCSS(a, "borderTopWidth", true)) || 0) + (parseFloat(jQuery.curCSS(a, "borderBottomWidth", true)) || 0) + (b ? tb(a) : 0)
    }
    function tb(a) {
        return (parseFloat(jQuery.curCSS(a, "marginTop", true)) || 0) + (parseFloat(jQuery.curCSS(a, "marginBottom", true)) || 0)
    }
    function gb(a, b) {
        b = typeof b == "number" ? b + "px" : b;
        a[0].style.cssText += ";min-height:" + b + ";_height:" + b
    }
    function nb(a) {
        var b = this,
			f, c;
        b.build = function () {
            f = [];
            c = [];
            a(f, c)
        };
        b.cell = function (g, n) {
            var k = f.length,
				y = c.length,
				w, t = -1,
				H = -1;
            for (w = 0; w < k; w++) if (n >= f[w][0] && n < f[w][1]) {
                t = w;
                break
            }
            for (w = 0; w < y; w++) if (g >= c[w][0] && g < c[w][1]) {
                H = w;
                break
            }
            return t >= 0 && H >= 0 ? {
                row: t,
                col: H
            } : null
        };
        b.rect = function (g, n, k, y, w) {
            w = w.offset();
            return {
                top: f[g][0] - w.top,
                left: c[n][0] - w.left,
                width: c[y][1] - c[n][0],
                height: f[k][1] - f[g][0]
            }
        }
    }

    function ob(a) {
        function b(y) {
            y = a.cell(y.pageX, y.pageY);
            if (!y != !k || y && (y.row != k.row || y.col != k.col)) {
                if (y) {
                    n || (n = y);
                    g(y, n, y.row - n.row, y.col - n.col)
                } else g(y, n);
                k = y
            }
        }
        var f = this,
			c, g, n, k;
        f.start = function (y, w, t) {
            g = y;
            n = k = null;
            a.build();
            b(w);
            c = t || "mousemove";
            o(document).bind(c, b)
        };
        f.stop = function () {
            o(document).unbind(c, b);
            return k
        }
    }
    function Na(a) {
        return (a < 10 ? "0" : "") + a
    }
    function hb(a, b) {
        if (a[b] !== K) return a[b];
        b = b.split(/(?=[A-Z])/);
        for (var f = b.length - 1, c; f >= 0; f--) {
            c = a[b[f].toLowerCase()];
            if (c !== K) return c
        }
        return a[""]
    }

    function Ka(a) {
        return a.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/'/g, "&#039;").replace(/"/g, "&quot;").replace(/\n/g, "<br />")
    }
    function kb(a) {
        function b(k) {
            return c[k] = c[k] || a(k)
        }
        var f = this,
			c = {},
			g = {},
			n = {};
        f.left = function (k) {
            return g[k] = g[k] === K ? b(k).position().left : g[k]
        };
        f.right = function (k) {
            return n[k] = n[k] === K ? f.left(k) + b(k).width() : n[k]
        };
        f.clear = function () {
            c = {};
            g = {};
            n = {}
        }
    }
    function sb(a) {
        return a.id + "/" + a.className + "/" + a.style.cssText.replace(/(^|;)\s*(top|left|width|height)\s*:[^;]*/ig, "")
    }
    function zb(a, b) {
        return a - b
    }
    function Pa(a) {
        return a.end ? Lb(a.end, a.allDay) : C(q(a.start), 1)
    }
    function Lb(a, b) {
        a = q(a);
        return b || a.getHours() || a.getMinutes() ? C(a, 1) : Ga(a)
    }
    function mb(a) {
        a.attr("unselectable", "on").css("MozUserSelect", "none").bind("selectstart.ui", function () {
            return false
        })
    }
    var ya = o.fullCalendar = {},
		Oa = ya.views = {},
		Va = {
		    defaultView: "month",
		    aspectRatio: 1.35,
		    header: {
		        left: "title",
		        center: "",
		        right: "today prev,next"
		    },
		    weekends: true,
		    allDayDefault: true,
		    lazyFetching: true,
		    startParam: "start",
		    endParam: "end",
		    titleFormat: {
		        month: "MMMM yyyy",
		        week: "MMM d[ yyyy]{ '&#8212;'[ MMM] d yyyy}",
		        day: "dddd, MMM d, yyyy"
		    },
		    columnFormat: {
		        month: "ddd",
		        week: "ddd M/d",
		        day: "dddd M/d"
		    },
		    timeFormat: {
		        "": "h(:mm)t"
		    },
		    isRTL: false,
		    firstDay: 0,
		    /*monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],*/
		    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
		    monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
		    /*dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],*/
		    dayNames: ["週日", "週一", "週二", "週三", "週四", "週五", "週六"],
		    /*dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],*/
		    dayNamesShort: ["日", "一", "二", "三", "四", "五", "六"],
		    buttonText: {
		        prev: "&nbsp;&#9668;&nbsp;",
		        next: "&nbsp;&#9658;&nbsp;",
		        prevYear: "&nbsp;&lt;&lt;&nbsp;",
		        nextYear: "&nbsp;&gt;&gt;&nbsp;",
		        today: "today",
		        month: "month",
		        week: "week",
		        day: "day"
		    },
		    theme: false,
		    buttonIcons: {
		        prev: "circle-triangle-w",
		        next: "circle-triangle-e"
		    },
		    unselectAuto: true,
		    dropAccept: "*"
		},
		Mb = {
		    header: {
		        left: "next,prev today",
		        center: "",
		        right: "title"
		    },
		    buttonText: {
		        prev: "&nbsp;&#9658;&nbsp;",
		        next: "&nbsp;&#9668;&nbsp;",
		        prevYear: "&nbsp;&gt;&gt;&nbsp;",
		        nextYear: "&nbsp;&lt;&lt;&nbsp;"
		    },
		    buttonIcons: {
		        prev: "circle-triangle-e",
		        next: "circle-triangle-w"
		    }
		},
		Db = ya.setDefaults = function (a) {
		    o.extend(true, Va, a)
		};
    o.fn.fullCalendar = function (a) {
        if (typeof a == "string") {
            var b = Array.prototype.slice.call(arguments, 1),
				f;
            this.each(function () {
                var n = o.data(this, "fullCalendar");
                if (n) if (n = n[a]) {
                    n = n.apply(this, b);
                    if (f === K) f = n
                }
            });
            if (f !== K) return f;
            return this
        }
        var c = a.eventSources || [];
        delete a.eventSources;
        if (a.events) {
            c.push(a.events);
            delete a.events
        }
        c.unshift([]);
        a = o.extend(true, {}, Va, a.isRTL || a.isRTL === K && Va.isRTL ? Mb : {}, a);
        var g = a.theme ? "ui" : "fc";
        this.each(function () {
            function n(e) {
                if (e != Aa) {
                    N++;
                    t();
                    var j = x,
						r;
                    if (j) {
                        if (j.eventsChanged) {
                            M();
                            j.eventDirty = j.eventsChanged = false
                        }
                        j.beforeHide && j.beforeHide();
                        gb(R, R.height());
                        j.element.hide()
                    } else gb(R, 1);
                    R.css("overflow", "hidden");
                    if (U[e]) (x = U[e]).element.show();
                    else x = U[e] = ya.views[e](r = wa = o("<div class='fc-view fc-view-" + e + "' style='position:absolute'/>").appendTo(R), a, e);
                    if ($) {
                        $.find("div.fc-button-" + Aa).removeClass(g + "-state-active");
                        $.find("div.fc-button-" + e).addClass(g + "-state-active")
                    }
                    Aa = e;
                    k();
                    R.css("overflow", "");
                    j && gb(R, 1);
                    !r && x.afterShow && x.afterShow();
                    N--
                }
            }
            function k(e) {
                if (y()) {
                    N++;
                    t();
                    va === K && ga();
                    if (!x.start || e || L < x.start || L >= x.end) {
                        x.render(L, e || 0);
                        X(true);
                        !la || !a.lazyFetching || x.visStart < la || x.visEnd > A ? Q() : x.renderEvents(P)
                    } else if (x.sizeDirty || x.eventsDirty || !a.lazyFetching) {
                        x.clearEvents();
                        x.sizeDirty && X();
                        a.lazyFetching ? x.renderEvents(P) : Q()
                    }
                    ka = ra.outerWidth();
                    x.sizeDirty = false;
                    x.eventsDirty = false;
                    if ($) {
                        $.find("h2.fc-header-title").html(x.title);
                        e = new Date;
                        e >= x.start && e < x.end ? $.find("div.fc-button-today").addClass(g + "-state-disabled") : $.find("div.fc-button-today").removeClass(g + "-state-disabled")
                    }
                    N--;
                    x.trigger("viewDisplay", O)
                }
            }
            function y() {
                return O.offsetWidth !== 0
            }
            function w() {
                return o("body")[0].offsetWidth !== 0
            }
            function t() {
                x && x.unselect()
            }
            function H() {
                M();
                if (y()) {
                    x.clearEvents();
                    x.renderEvents(P);
                    x.eventsDirty = false
                }
            }
            function M() {
                o.each(U, function () {
                    this.eventsDirty = true
                })
            }
            function sa() {
                G();
                if (y()) {
                    ga();
                    X();
                    t();
                    x.rerenderEvents();
                    x.sizeDirty = false
                }
            }
            function G() {
                o.each(U, function () {
                    this.sizeDirty = true
                })
            }
            function fa(e) {
                P = [];
                la = q(x.visStart);
                A = q(x.visEnd);
                for (var j = c.length, r = function () {
                    --j || e && e(P)
                }, s = 0; s < c.length; s++) D(c[s], r)
            }
            function D(e, j) {
                var r = x.name,
					s = q(L),
					I = function (J) {
					    if (r == x.name && +s == +L && o.inArray(e, c) != -1) {
					        for (var W = 0; W < J.length; W++) {
					            Ia(J[W], a);
					            J[W].source = e
					        }
					        P = P.concat(J);
					        j && j(J)
					    }
					},
					E = function (J) {
					    I(J);
					    ma()
					};
                if (typeof e == "string") {
                    var B = {};
                    B[a.startParam] = Math.round(la.getTime() / 1E3);
                    B[a.endParam] = Math.round(A.getTime() / 1E3);
                    if (a.cacheParam) B[a.cacheParam] = (new Date).getTime();
                    ha();
                    o.ajax({
                        url: e,
                        dataType: "json",
                        data: B,
                        cache: a.cacheParam || false,
                        success: E
                    })
                } else if (o.isFunction(e)) {
                    ha();
                    e(q(la), q(A), E)
                } else I(e)
            }
            function Q() {
                fa(function (e) {
                    x.renderEvents(e)
                })
            }
            function ha() {
                aa++ || x.trigger("loading", O, true)
            }
            function ma() {
                --aa || x.trigger("loading", O, false)
            }
            function pa(e) {
                if (e) {
                    var j = o("<tr/>");
                    o.each(e.split(" "), function (r) {
                        r > 0 && j.append("<td><span class='fc-header-space'/></td>");
                        var s;
                        o.each(this.split(","), function (I, E) {
                            if (E == "title") {
                                j.append("<td><h2 class='fc-header-title'>&nbsp;</h2></td>");
                                s && s.addClass(g + "-corner-right");
                                s = null
                            } else {
                                var B;
                                if (Z[E]) B = Z[E];
                                else if (Oa[E]) B = function () {
                                    J.removeClass(g + "-state-hover");
                                    n(E)
                                };
                                if (B) {
                                    s && s.addClass(g + "-no-right");
                                    var J;
                                    I = a.theme ? hb(a.buttonIcons, E) : null;
                                    var W = hb(a.buttonText, E);
                                    if (I) J = o("<div class='fc-button-" + E + " ui-state-default'><a><span class='ui-icon ui-icon-" + I + "'/></a></div>");
                                    else if (W) J = o("<div class='fc-button-" + E + " " + g + "-state-default'><a><span>" + W + "</span></a></div>");
                                    if (J) {
                                        J.click(function () {
                                            J.hasClass(g + "-state-disabled") || B()
                                        }).mousedown(function () {
                                            J.not("." + g + "-state-active").not("." + g + "-state-disabled").addClass(g + "-state-down")
                                        }).mouseup(function () {
                                            J.removeClass(g + "-state-down")
                                        }).hover(function () {
                                            J.not("." + g + "-state-active").not("." + g + "-state-disabled").addClass(g + "-state-hover")
                                        }, function () {
                                            J.removeClass(g + "-state-hover").removeClass(g + "-state-down")
                                        }).appendTo(o("<td/>").appendTo(j));
                                        s ? s.addClass(g + "-no-right") : J.addClass(g + "-corner-left");
                                        s = J
                                    }
                                }
                            }
                        });
                        s && s.addClass(g + "-corner-right")
                    });
                    return o("<table/>").append(j)
                }
            }
            function ga() {
                va = a.contentHeight ? a.contentHeight : a.height ? a.height - ($ ? $.height() : 0) - bb(R[0]) : Math.round(R.width() / Math.max(a.aspectRatio, 0.5))
            }
            function X(e) {
                N++;
                x.setHeight(va, e);
                if (wa) {
                    wa.css("position", "relative");
                    wa = null
                }
                x.setWidth(R.width(), e);
                N--
            }
            function ca() {
                if (!N) if (x.start) {
                    var e = ++Fa;
                    setTimeout(function () {
                        if (e == Fa && !N && y()) if (ka != (ka = ra.outerWidth())) {
                            N++;
                            sa();
                            x.trigger("windowResize", O);
                            N--
                        }
                    }, 200)
                } else ua()
            }
            function ua() {
                setTimeout(function () {
                    !x.start && w() && k()
                }, 0)
            }
            var O = this,
				ra = o(O).addClass("fc"),
				ka, R = o("<div class='fc-content " + g + "-widget-content' style='position:relative'/>").prependTo(O),
				va, Fa = 0,
				N = 0,
				L = new Date,
				Aa, x, U = {},
				wa;
            a.isRTL && ra.addClass("fc-rtl");
            a.theme && ra.addClass("ui-widget");
            Cb(L, a.year, a.month, a.date);
            var P = [],
				la, A, aa = 0,
				Z = {
				    render: function () {
				        ga();
				        G();
				        M();
				        k()
				    },
				    changeView: n,
				    getView: function () {
				        return x
				    },
				    getDate: function () {
				        return L
				    },
				    option: function (e, j) {
				        if (j === K) return a[e];
				        if (e == "height" || e == "contentHeight" || e == "aspectRatio") {
				            a[e] = j;
				            sa()
				        }
				    },
				    destroy: function () {
				        o(window).unbind("resize", ca);
				        $ && $.remove();
				        R.remove();
				        o.removeData(O, "fullCalendar")
				    },
				    prev: function () {
				        k(-1)
				    },
				    next: function () {
				        k(1)
				    },
				    prevYear: function () {
				        db(L, -1);
				        k()
				    },
				    nextYear: function () {
				        db(L, 1);
				        k()
				    },
				    today: function () {
				        L = new Date;
				        k()
				    },
				    gotoDate: function (e, j, r) {
				        if (typeof e == "object") L = q(e);
				        else Cb(L, e, j, r);
				        k()
				    },
				    incrementDate: function (e, j, r) {
				        e !== K && db(L, e);
				        j !== K && eb(L, j);
				        r !== K && C(L, r);
				        k()
				    },
				    updateEvent: function (e) {
				        var j, r = P.length,
							s, I = e.start - e._start,
							E = e.end ? e.end - (e._end || x.defaultEventEnd(e)) : 0;
				        for (j = 0; j < r; j++) {
				            s = P[j];
				            if (s._id == e._id && s != e) {
				                s.start = new Date(+s.start + I);
				                s.end = e.end ? s.end ? new Date(+s.end + E) : new Date(+x.defaultEventEnd(s) + E) : null;
				                s.title = e.title;
				                s.url = e.url;
				                s.allDay = e.allDay;
				                s.className = e.className;
				                s.editable = e.editable;
				                Ia(s, a)
				            }
				        }
				        Ia(e, a);
				        H()
				    },
				    renderEvent: function (e, j) {
				        Ia(e, a);
				        if (!e.source) {
				            if (j) (e.source = c[0]).push(e);
				            P.push(e)
				        }
				        H()
				    },
				    removeEvents: function (e) {
				        if (e) {
				            if (!o.isFunction(e)) {
				                var j = e + "";
				                e = function (s) {
				                    return s._id == j
				                }
				            }
				            P = o.grep(P, e, true);
				            for (r = 0; r < c.length; r++) if (typeof c[r] == "object") c[r] = o.grep(c[r], e, true)
				        } else {
				            P = [];
				            for (var r = 0; r < c.length; r++) if (typeof c[r] == "object") c[r] = []
				        }
				        H()
				    },
				    clientEvents: function (e) {
				        if (o.isFunction(e)) return o.grep(P, e);
				        else if (e) {
				            e += "";
				            return o.grep(P, function (j) {
				                return j._id == e
				            })
				        }
				        return P
				    },
				    rerenderEvents: H,
				    addEventSource: function (e) {
				        c.push(e);
				        D(e, H)
				    },
				    removeEventSource: function (e) {
				        c = o.grep(c, function (j) {
				            return j != e
				        });
				        P = o.grep(P, function (j) {
				            return j.source != e
				        });
				        H()
				    },
				    refetchEvents: function () {
				        fa(H)
				    },
				    select: function (e, j, r) {
				        x.select(e, j, r === K ? true : r)
				    },
				    unselect: function () {
				        x.unselect()
				    }
				};
            o.data(this, "fullCalendar", Z);
            var $, ia = a.header;
            if (ia) $ = o("<table class='fc-header'/>").append(o("<tr/>").append(o("<td class='fc-header-left'/>").append(pa(ia.left))).append(o("<td class='fc-header-center'/>").append(pa(ia.center))).append(o("<td class='fc-header-right'/>").append(pa(ia.right)))).prependTo(ra);
            o(window).resize(ca);
            if (a.droppable) {
                var m;
                o(document).bind("dragstart", function (e, j) {
                    var r = e.target,
						s = o(r);
                    if (!s.parents(".fc").length) {
                        var I = a.dropAccept;
                        if (o.isFunction(I) ? I.call(r, s) : s.is(I)) {
                            m = r;
                            x.dragStart(m, e, j)
                        }
                    }
                }).bind("dragstop", function (e, j) {
                    if (m) {
                        x.dragStop(m, e, j);
                        m = null
                    }
                })
            }
            n(a.defaultView);
            w() || ua()
        });
        return this
    };
    var Eb = 0;
    Db({
        weekMode: "fixed"
    });
    Oa.month = function (a, b, f) {
        return new Wa(a, b, {
            render: function (c, g) {
                if (g) {
                    eb(c, g);
                    c.setDate(1)
                }
                c = this.start = q(c, true);
                c.setDate(1);
                this.end = eb(q(c), 1);
                var n = this.visStart = q(c);
                g = this.visEnd = q(this.end);
                var k = b.weekends ? 0 : 1;
                if (k) {
                    da(n);
                    da(g, -1, true)
                }
                C(n, -((n.getDay() - Math.max(b.firstDay, k) + 7) % 7));
                C(g, (7 - g.getDay() + Math.max(b.firstDay, k)) % 7);
                n = Math.round((g - n) / (Bb * 7));
                if (b.weekMode == "fixed") {
                    C(g, (6 - n) * 7);
                    n = 6
                }
                this.title = oa(c, this.option("titleFormat"), b);
                this.renderGrid(n, b.weekends ? 7 : 5, this.option("columnFormat"), true)
            }
        }, f)
    };
    Oa.basicWeek = function (a, b, f) {
        return new Wa(a, b, {
            render: function (c, g) {
                g && C(c, g * 7);
                c = this.visStart = q(this.start = C(q(c), -((c.getDay() - b.firstDay + 7) % 7)));
                g = this.visEnd = q(this.end = C(q(c), 7));
                if (!b.weekends) {
                    da(c);
                    da(g, -1, true)
                }
                this.title = Ha(c, C(q(g), -1), this.option("titleFormat"), b);
                this.renderGrid(1, b.weekends ? 7 : 5, this.option("columnFormat"), false)
            }
        }, f)
    };
    Oa.basicDay = function (a, b, f) {
        return new Wa(a, b, {
            render: function (c, g) {
                if (g) {
                    C(c, g);
                    b.weekends || da(c, g < 0 ? -1 : 1)
                }
                this.title = oa(c, this.option("titleFormat"), b);
                this.start = this.visStart = q(c, true);
                this.end = this.visEnd = C(q(this.start), 1);
                this.renderGrid(1, 1, this.option("columnFormat"), false)
            }
        }, f)
    };
    var Xa;
    Db({
        allDaySlot: true,
        allDayText: "all-day",
        firstHour: 6,
        slotMinutes: 30,
        defaultEventMinutes: 120,
        axisFormat: "h(:mm)tt",
        timeFormat: {
            agenda: "h:mm{ - h:mm}"
        },
        dragOpacity: {
            agenda: 0.5
        },
        minTime: 0,
        maxTime: 24
    });
    Oa.agendaWeek = function (a, b, f) {
        return new ub(a, b, {
            render: function (c, g) {
                g && C(c, g * 7);
                c = this.visStart = q(this.start = C(q(c), -((c.getDay() - b.firstDay + 7) % 7)));
                g = this.visEnd = q(this.end = C(q(c), 7));
                if (!b.weekends) {
                    da(c);
                    da(g, -1, true)
                }
                this.title = Ha(c, C(q(g), -1), this.option("titleFormat"), b);
                this.renderAgenda(b.weekends ? 7 : 5, this.option("columnFormat"))
            }
        }, f)
    };
    Oa.agendaDay = function (a, b, f) {
        return new ub(a, b, {
            render: function (c, g) {
                if (g) {
                    C(c, g);
                    b.weekends || da(c, g < 0 ? -1 : 1)
                }
                this.title = oa(c, this.option("titleFormat"), b);
                this.start = this.visStart = q(c, true);
                this.end = this.visEnd = C(q(this.start), 1);
                this.renderAgenda(1, this.option("columnFormat"))
            }
        }, f)
    };
    var lb = {
        init: function (a, b) {
            this.element = a;
            this.options = b;
            this.eventsByID = {};
            this.eventElements = [];
            this.eventElementsByID = {};
            this.usedOverlays = [];
            this.unusedOverlays = []
        },
        trigger: function (a, b) {
            if (this.options[a]) return this.options[a].apply(b || this, Array.prototype.slice.call(arguments, 2).concat([this]))
        },
        eventEnd: function (a) {
            return a.end ? q(a.end) : this.defaultEventEnd(a)
        },
        reportEvents: function (a) {
            var b, f = a.length,
				c, g = this.eventsByID = {};
            for (b = 0; b < f; b++) {
                c = a[b];
                if (g[c._id]) g[c._id].push(c);
                else g[c._id] = [c]
            }
        },
        reportEventElement: function (a, b) {
            this.eventElements.push(b);
            var f = this.eventElementsByID;
            if (f[a._id]) f[a._id].push(b);
            else f[a._id] = [b]
        },
        _clearEvents: function () {
            this.eventElements = [];
            this.eventElementsByID = {}
        },
        showEvents: function (a, b) {
            this._eee(a, b, "show")
        },
        hideEvents: function (a, b) {
            this._eee(a, b, "hide")
        },
        _eee: function (a, b, f) {
            a = this.eventElementsByID[a._id];
            var c, g = a.length;
            for (c = 0; c < g; c++) a[c][0] != b[0] && a[c][f]()
        },
        eventDrop: function (a, b, f, c, g, n, k) {
            var y = this,
				w = b.allDay,
				t = b._id;
            y.moveEvents(y.eventsByID[t], f, c, g);
            y.trigger("eventDrop", a, b, f, c, g, function () {
                y.moveEvents(y.eventsByID[t], -f, -c, w);
                y.rerenderEvents()
            }, n, k);
            y.eventsChanged = true;
            y.rerenderEvents(t)
        },
        eventResize: function (a, b, f, c, g, n) {
            var k = this,
				y = b._id;
            k.elongateEvents(k.eventsByID[y], f, c);
            k.trigger("eventResize", a, b, f, c, function () {
                k.elongateEvents(k.eventsByID[y], -f, -c);
                k.rerenderEvents()
            }, g, n);
            k.eventsChanged = true;
            k.rerenderEvents(y)
        },
        moveEvents: function (a, b, f, c) {
            f = f || 0;
            for (var g, n = a.length, k = 0; k < n; k++) {
                g = a[k];
                if (c !== K) g.allDay = c;
                ba(C(g.start, b, true), f);
                if (g.end) g.end = ba(C(g.end, b, true), f);
                Ia(g, this.options)
            }
        },
        elongateEvents: function (a, b, f) {
            f = f || 0;
            for (var c, g = a.length, n = 0; n < g; n++) {
                c = a[n];
                c.end = ba(C(this.eventEnd(c), b, true), f);
                Ia(c, this.options)
            }
        },
        renderOverlay: function (a, b) {
            var f = this.unusedOverlays.shift();
            f || (f = o("<div class='fc-cell-overlay' style='position:absolute;z-index:3'/>"));
            f[0].parentNode != b[0] && f.appendTo(b);
            this.usedOverlays.push(f.css(a).show());
            return f
        },
        clearOverlays: function () {
            for (var a; a = this.usedOverlays.shift(); ) this.unusedOverlays.push(a.hide().unbind())
        },
        resizableDayEvent: function (a, b, f) {
            var c = this;
            if (!c.options.disableResizing && b.resizable) b.resizable({
                handles: c.options.isRTL ? {
                    w: "div.ui-resizable-w"
                } : {
                    e: "div.ui-resizable-e"
                },
                grid: f,
                minWidth: f / 2,
                containment: c.element.parent().parent(),
                start: function (g, n) {
                    b.css("z-index", 9);
                    c.hideEvents(a, b);
                    c.trigger("eventResizeStart", this, a, g, n)
                },
                stop: function (g, n) {
                    c.trigger("eventResizeStop", this, a, g, n);
                    var k = Math.round((b.width() - n.originalSize.width) / f);
                    if (k) c.eventResize(this, a, k, 0, g, n);
                    else {
                        b.css("z-index", 8);
                        c.showEvents(a, b)
                    }
                }
            })
        },
        eventElementHandlers: function (a, b) {
            var f = this;
            b.click(function (c) {
                if (!b.hasClass("ui-draggable-dragging") && !b.hasClass("ui-resizable-resizing")) return f.trigger("eventClick", this, a, c)
            }).hover(function (c) {
                f.trigger("eventMouseover", this, a, c)
            }, function (c) {
                f.trigger("eventMouseout", this, a, c)
            })
        },
        option: function (a, b) {
            a = this.options[a];
            if (typeof a == "object") return hb(a, b || this.name);
            return a
        },
        sliceSegs: function (a, b, f, c) {
            var g = [],
				n, k = a.length,
				y, w, t, H, M;
            for (n = 0; n < k; n++) {
                y = a[n];
                w = y.start;
                t = b[n];
                if (t > f && w < c) {
                    if (w < f) {
                        w = q(f);
                        H = false
                    } else {
                        w = w;
                        H = true
                    }
                    if (t > c) {
                        t = q(c);
                        M = false
                    } else {
                        t = t;
                        M = true
                    }
                    g.push({
                        event: y,
                        start: w,
                        end: t,
                        isStart: H,
                        isEnd: M,
                        msLength: t - w
                    })
                }
            }
            return g.sort(Ib)
        }
    },
		Bb = 864E5,
		Jb = 36E5,
		Hb = 6E4;
    ya.addDays = C;
    ya.cloneDate = q;
    var ib = ya.parseDate = function (a) {
        if (typeof a == "object") return a;
        if (typeof a == "number") return new Date(a * 1E3);
        if (typeof a == "string") {
            if (a.match(/^\d+$/)) return new Date(parseInt(a) * 1E3);
            return Nb(a, true) || (a ? new Date(a) : null)
        }
        return null
    },
		Nb = ya.parseISO8601 = function (a, b) {
		    a = a.match(/^([0-9]{4})(-([0-9]{2})(-([0-9]{2})([T ]([0-9]{2}):([0-9]{2})(:([0-9]{2})(\.([0-9]+))?)?(Z|(([-+])([0-9]{2}):([0-9]{2})))?)?)?)?$/);
		    if (!a) return null;
		    var f = new Date(a[1], 0, 1),
				c = new Date(a[1], 0, 1, 9, 0),
				g = 0;
		    if (a[3]) {
		        f.setMonth(a[3] - 1);
		        c.setMonth(a[3] - 1)
		    }
		    if (a[5]) {
		        f.setDate(a[5]);
		        c.setDate(a[5])
		    }
		    fb(f, c);
		    a[7] && f.setHours(a[7]);
		    a[8] && f.setMinutes(a[8]);
		    a[10] && f.setSeconds(a[10]);
		    a[12] && f.setMilliseconds(Number("0." + a[12]) * 1E3);
		    fb(f, c);
		    if (!b) {
		        if (a[14]) {
		            g = Number(a[16]) * 60 + Number(a[17]);
		            g *= a[15] == "-" ? 1 : -1
		        }
		        g -= f.getTimezoneOffset()
		    }
		    return new Date(+f + g * 60 * 1E3)
		},
		wb = ya.parseTime = function (a) {
		    if (typeof a == "number") return a * 60;
		    if (typeof a == "object") return a.getHours() * 60 + a.getMinutes();
		    if (a = a.match(/(\d+)(?::(\d+))?\s*(\w+)?/)) {
		        var b = parseInt(a[1]);
		        if (a[3]) {
		            b %= 12;
		            if (a[3].toLowerCase().charAt(0) == "p") b += 12
		        }
		        return b * 60 + (a[2] ? parseInt(a[2]) : 0)
		    }
		},
		oa = ya.formatDate = function (a, b, f) {
		    return Ha(a, null, b, f)
		},
		Ha = ya.formatDates = function (a, b, f, c) {
		    c = c || Va;
		    var g = a,
				n = b,
				k, y = f.length,
				w, t, H, M = "";
		    for (k = 0; k < y; k++) {
		        w = f.charAt(k);
		        if (w == "'") for (t = k + 1; t < y; t++) {
		            if (f.charAt(t) == "'") {
		                if (g) {
		                    M += t == k + 1 ? "'" : f.substring(k + 1, t);
		                    k = t
		                }
		                break
		            }
		        } else if (w == "(") for (t = k + 1; t < y; t++) {
		            if (f.charAt(t) == ")") {
		                k = oa(g, f.substring(k + 1, t), c);
		                if (parseInt(k.replace(/\D/, ""))) M += k;
		                k = t;
		                break
		            }
		        } else if (w == "[") for (t = k + 1; t < y; t++) {
		            if (f.charAt(t) == "]") {
		                w = f.substring(k + 1, t);
		                k = oa(g, w, c);
		                if (k != oa(n, w, c)) M += k;
		                k = t;
		                break
		            }
		        } else if (w == "{") {
		            g = b;
		            n = a
		        } else if (w == "}") {
		            g = a;
		            n = b
		        } else {
		            for (t = y; t > k; t--) if (H = Ob[f.substring(k, t)]) {
		                if (g) M += H(g, c);
		                k = t - 1;
		                break
		            }
		            if (t == k) if (g) M += w
		        }
		    }
		    return M
		},
		Ob = {
		    s: function (a) {
		        return a.getSeconds()
		    },
		    ss: function (a) {
		        return Na(a.getSeconds())
		    },
		    m: function (a) {
		        return a.getMinutes()
		    },
		    mm: function (a) {
		        return Na(a.getMinutes())
		    },
		    h: function (a) {
		        return a.getHours() % 12 || 12
		    },
		    hh: function (a) {
		        return Na(a.getHours() % 12 || 12)
		    },
		    H: function (a) {
		        return a.getHours()
		    },
		    HH: function (a) {
		        return Na(a.getHours())
		    },
		    d: function (a) {
		        return a.getDate()
		    },
		    dd: function (a) {
		        return Na(a.getDate())
		    },
		    ddd: function (a, b) {
		        return b.dayNamesShort[a.getDay()]
		    },
		    dddd: function (a, b) {
		        return b.dayNames[a.getDay()]
		    },
		    M: function (a) {
		        return a.getMonth() + 1
		    },
		    MM: function (a) {
		        return Na(a.getMonth() + 1)
		    },
		    MMM: function (a, b) {
		        return b.monthNamesShort[a.getMonth()]
		    },
		    MMMM: function (a, b) {
		        return b.monthNames[a.getMonth()]
		    },
		    yy: function (a) {
		        return (a.getFullYear() + "").substring(2)
		    },
		    yyyy: function (a) {
		        return a.getFullYear()
		    },
		    t: function (a) {
		        return a.getHours() < 12 ? "a" : "p"
		    },
		    tt: function (a) {
		        return a.getHours() < 12 ? "am" : "pm"
		    },
		    T: function (a) {
		        return a.getHours() < 12 ? "A" : "P"
		    },
		    TT: function (a) {
		        return a.getHours() < 12 ? "AM" : "PM"
		    },
		    u: function (a) {
		        return oa(a, "yyyy-MM-dd'T'HH:mm:ss'Z'")
		    },
		    S: function (a) {
		        a = a.getDate();
		        if (a > 10 && a < 20) return "th";
		        return ["st", "nd", "rd"][a % 10 - 1] || "th"
		    }
		},
		za = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"]
})(jQuery);
