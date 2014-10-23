$(document).ready(function () {
    /* Core JS Functions */

    /* Collapsible Panels */
    $(".mws-panel.mws-collapsible .mws-panel-header")
		.append("<div class=\"mws-collapse-button mws-inset\"><span></span></div>")
			.find(".mws-collapse-button span")
				.on("click", function (event) {
				    $(this).toggleClass("mws-collapsed")
						.parents(".mws-panel")
							.find(".mws-panel-body")
								.slideToggle("fast");
				});

    /* Side dropdown menu */
    $("div#mws-navigation ul li a, div#mws-navigation ul li span")
	.bind('click', function (event) {
	    //debugger;

	    var parent = $(this).parent();
	    var parentLevel = parent.attr("data-level");

	    if (parentLevel && parentLevel == "1") {
	        $("div#mws-navigation ul li[data-level='1']").each(function (i, e) {
	            $(this).removeClass("active");
	        });

	        $("div#mws-navigation ul li[data-level='1'] ul").each(function (index, e) {
	            $(this).hide();
	        });

	        parent.addClass("active");
	    }
	    else {
	        $("div#mws-navigation ul li ul li").each(function (i, e) {
	            if ($(this).attr("data-level") == parentLevel) {
	                $(this).removeClass("active-sub-nav");
	            }
	        });

	        var curRootNav = parent.parent().parent();

	        if (curRootNav && !curRootNav.hasClass("active")) {

	            $("div#mws-navigation ul li[data-level='1']").each(function (i, e) {
	                $(this).removeClass("active");
	            });

	            curRootNav.addClass("active");
	        }

	        parent.addClass("active-sub-nav");
	    }

	    if ($(this).next('ul').size() !== 0) {
	        $(this).next('ul').slideToggle('fast', function () {
	            $(this).toggleClass('closed');
	        });
	        event.preventDefault();
	    }
	});

    /* Message & Notifications Dropdown */
    $("div#mws-user-tools .mws-dropdown-menu a").click(function (event) {
        $(".mws-dropdown-menu.toggled").not($(this).parent()).removeClass("toggled");
        $(this).parent().toggleClass("toggled");
        event.preventDefault();
    });

    $('html').click(function (event) {
        if ($(event.target).parents('.mws-dropdown-menu').size() == 0) {
            $(".mws-dropdown-menu").removeClass("toggled");
        }
    });

    /* Side Menu Notification Class */
    $(".mws-nav-tooltip").addClass("mws-inset");

    /* Table Row CSS Class */
    $("table.mws-table tbody tr:even").addClass("even");
    $("table.mws-table tbody tr:odd").addClass("odd");

    /* File Input Styling */

    if ($.fn.filestyle) {
        $("input[type='file']").filestyle({
            imagewidth: 78,
            imageHeight: 28
        });
        $("input.file").attr("readonly", true);
    }

    /* Tooltips */

    if ($.fn.tipsy) {
        var gravity = ['n', 'ne', 'e', 'se', 's', 'sw', 'w', 'nw'];
        for (var i in gravity)
            $(".mws-tooltip-" + gravity[i]).tipsy({ gravity: gravity[i] });

        $('input[title], select[title], textarea[title]').tipsy({ trigger: 'focus', gravity: 'w' });
    }

    /* Dual List Box */

    if ($.configureBoxes) {
        $.configureBoxes();
    }

    if ($.fn.placeholder) {
        $('[placeholder]').placeholder();
    }
});
