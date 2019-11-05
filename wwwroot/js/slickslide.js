$(document).ready(function () {
	/*	Coming soon movies slider
		It uses Slick Slider for the movie roller.
	*/
	$('#soon-slider').slick({
		autoplay: true,
		arrows: false,
		autoplaySpeed: 5000,
		centerMode: true,
		centerPadding: '5%',
		slidesToShow: 5,
		responsive: [
			{
				breakpoint: 1000,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '0',
					slidesToShow: 4
				}
			},
			{
				breakpoint: 768,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '5%',
					slidesToShow: 3
				}
			},
			{
				breakpoint: 768,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '0',
					slidesToShow: 3
				}
			},
			{
				breakpoint: 700,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '10%',
					slidesToShow: 2
				}
			},
			{
				breakpoint: 576,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '0',
					slidesToShow: 2
				}
			},
			{
				breakpoint: 500,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '15%',
					slidesToShow: 1
				}
			},
			{
				breakpoint: 340,
				settings: {
					arrows: false,
					centerMode: true,
					centerPadding: '0',
					slidesToShow: 1
				}
			}
		]

        /*
         accessibility
         boolean
         true
         Enables tabbing and arrow key navigation

         arrows
         boolean
         true
         Prev/Next Arrows

         asNavFor
         string
         null
         Set the slider to be the navigation of other slider (Class or ID Name)

         appendArrows
         string
         $(element)
         Change where the navigation arrows are attached (Selector, htmlString, Array, Element, jQuery object)

         appendDots
         string
         $(element)
         Change where the navigation dots are attached (Selector, htmlString, Array, Element, jQuery object)

         prevArrow
         string (html|jQuery selector) | object (DOM node|jQuery object)
         <button type="button" class="slick-prev">Previous</button>
         Allows you to select a node or customize the HTML for the "Previous" arrow.

         nextArrow
         string (html|jQuery selector) | object (DOM node|jQuery object)
         <button type="button" class="slick-next">Next</button>
         Allows you to select a node or customize the HTML for the "Next" arrow.

         centerMode
         boolean
         false
         Enables centered view with partial prev/next slides. Use with odd numbered slidesToShow counts.

         centerPadding
         string
         '50px'
         Side padding when in center mode (px or %)

         cssEase
         string
         'ease'
         CSS3 Animation Easing

         customPaging
         function
         n/a
         Custom paging templates. See source for use example.

         dots
         boolean
         false
         Show dot indicators

         dotsClass
         string
         'slick-dots'
         Class for slide indicator dots container

         draggable
         boolean
         true
         Enable mouse dragging

         fade
         boolean
         false
         Enable fade

         focusOnSelect
         boolean
         false
         Enable focus on selected element (click)

         easing
         string
         'linear'
         Add easing for jQuery animate. Use with easing libraries or default easing methods

         edgeFriction
         integer
         0.15
         Resistance when swiping edges of non-infinite carousels

         infinite
         boolean
         true
         Infinite loop sliding

         initialSlide
         integer
         0
         Slide to start on

         lazyLoad
         string
         'ondemand'
         Set lazy loading technique. Accepts 'ondemand' or 'progressive'

         mobileFirst
         boolean
         false
         Responsive settings use mobile first calculation

         pauseOnFocus
         boolean
         true
         Pause Autoplay On Focus

         pauseOnHover
         boolean
         true
         Pause Autoplay On Hover

         pauseOnDotsHover
         boolean
         false
         Pause Autoplay when a dot is hovered

         respondTo
         string
         'window'
         Width that responsive object responds to. Can be 'window', 'slider' or 'min' (the smaller of the two)

         responsive
         object
         none
         Object containing breakpoints and settings objects (see demo). Enables settings sets at given screen width. Set settings to "unslick" instead of an object to disable slick at a given breakpoint.

         rows
         int
         1
         Setting this to more than 1 initializes grid mode. Use slidesPerRow to set how many slides should be in each row.

         slide
         element
         ''
         Element query to use as slide

         slidesPerRow
         int
         1
         With grid mode intialized via the rows option, this sets how many slides are in each grid row. dver

         slidesToShow
         int
         1
         # of slides to show

         slidesToScroll
         int
         1
         # of slides to scroll

         speed
         int(ms)
         300
         Slide/Fade animation speed

         swipe
         boolean
         true
         Enable swiping

         swipeToSlide
         boolean
         false
         Allow users to drag or swipe directly to a slide irrespective of slidesToScroll

         touchMove
         boolean
         true
         Enable slide motion with touch

         touchThreshold
         int
         5
         To advance slides, the user must swipe a length of (1/touchThreshold) * the width of the slider

         useCSS
         boolean
         true
         Enable/Disable CSS Transitions

         useTransform
         boolean
         true
         Enable/Disable CSS Transforms

         variableWidth
         boolean
         false
         Variable width slides

         vertical
         boolean
         false
         Vertical slide mode

         verticalSwiping
         boolean
         false
         Vertical swipe mode

         rtl
         boolean
         false
         Change the slider's direction to become right-to-left

         waitForAnimate
         boolean
         true
         Ignores requests to advance the slide while animating

         zIndex
         number
         1000
         Set the zIndex values for slides, useful for IE9 and lower
         */

        /*
         accessibility
         boolean
         true
         Enables tabbing and arrow key navigation
         adaptiveHeight
         boolean
         false
         Enables adaptive height for single slide horizontal carousels.
         autoplay
         boolean
         false
         Enables Autoplay
         autoplaySpeed
         int(ms)
         3000
         Autoplay Speed in milliseconds
         arrows
         boolean
         true
         Prev/Next Arrows
         asNavFor
         string
         null
         Set the slider to be the navigation of other slider (Class or ID Name)
         appendArrows
         string
         $(element)
         Change where the navigation arrows are attached (Selector, htmlString, Array, Element, jQuery object)
         appendDots
         string
         $(element)
         Change where the navigation dots are attached (Selector, htmlString, Array, Element, jQuery object)
         prevArrow
         string (html|jQuery selector) | object (DOM node|jQuery object)
         <button type="button" class="slick-prev">Previous</button>
         Allows you to select a node or customize the HTML for the "Previous" arrow.
         nextArrow
         string (html|jQuery selector) | object (DOM node|jQuery object)
         <button type="button" class="slick-next">Next</button>
         Allows you to select a node or customize the HTML for the "Next" arrow.
         centerMode
         boolean
         false
         Enables centered view with partial prev/next slides. Use with odd numbered slidesToShow counts.
         centerPadding
         string
         '50px'
         Side padding when in center mode (px or %)
         cssEase
         string
         'ease'
         CSS3 Animation Easing
         customPaging
         function
         n/a
         Custom paging templates. See source for use example.
         dots
         boolean
         false
         Show dot indicators
         dotsClass
         string
         'slick-dots'
         Class for slide indicator dots container
         draggable
         boolean
         true
         Enable mouse dragging
         fade
         boolean
         false
         Enable fade
         focusOnSelect
         boolean
         false
         Enable focus on selected element (click)
         easing
         string
         'linear'
         Add easing for jQuery animate. Use with easing libraries or default easing methods
         edgeFriction
         integer
         0.15
         Resistance when swiping edges of non-infinite carousels
         infinite
         boolean
         true
         Infinite loop sliding
         initialSlide
         integer
         0
         Slide to start on
         lazyLoad
         string
         'ondemand'
         Set lazy loading technique. Accepts 'ondemand' or 'progressive'
         mobileFirst
         boolean
         false
         Responsive settings use mobile first calculation
         pauseOnFocus
         boolean
         true
         Pause Autoplay On Focus
         pauseOnHover
         boolean
         true
         Pause Autoplay On Hover
         pauseOnDotsHover
         boolean
         false
         Pause Autoplay when a dot is hovered
         respondTo
         string
         'window'
         Width that responsive object responds to. Can be 'window', 'slider' or 'min' (the smaller of the two)
         responsive
         object
         none
         Object containing breakpoints and settings objects (see demo). Enables settings sets at given screen width. Set settings to "unslick" instead of an object to disable slick at a given breakpoint.
         rows
         int
         1
         Setting this to more than 1 initializes grid mode. Use slidesPerRow to set how many slides should be in each row.
         slide
         element
         ''
         Element query to use as slide
         slidesPerRow
         int
         1
         With grid mode intialized via the rows option, this sets how many slides are in each grid row. dver
         slidesToShow
         int
         1
         # of slides to show
         slidesToScroll
         int
         1
         # of slides to scroll
         speed
         int(ms)
         300
         Slide/Fade animation speed
         swipe
         boolean
         true
         Enable swiping
         swipeToSlide
         boolean
         false
         Allow users to drag or swipe directly to a slide irrespective of slidesToScroll
         touchMove
         boolean
         true
         Enable slide motion with touch
         touchThreshold
         int
         5
         To advance slides, the user must swipe a length of (1/touchThreshold) * the width of the slider
         useCSS
         boolean
         true
         Enable/Disable CSS Transitions
         useTransform
         boolean
         true
         Enable/Disable CSS Transforms
         variableWidth
         boolean
         false
         Variable width slides
         vertical
         boolean
         false
         Vertical slide mode
         verticalSwiping
         boolean
         false
         Vertical swipe mode
         rtl
         boolean
         false
         Change the slider's direction to become right-to-left
         waitForAnimate
         boolean
         true
         Ignores requests to advance the slide while animating
         zIndex
         number
         1000
         Set the zIndex values for slides, useful for IE9 and lower
         */
	});
});