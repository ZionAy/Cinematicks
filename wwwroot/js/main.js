$(function () {
	/*  Youtube modal handler
        It uses the same modal window with different open buttons.
        Each button pass data attribute for the youtube video link.
        When the modal window opens the script adds the youtube link as "src".
        When the modal window closes the script removes the "src".
    */
	$('#modal-trailer').on('show.bs.modal', function (event) {
		var button = $(event.relatedTarget);
		var video = button.data('youtube');
		var options = '?autoplay=1&controls=1&iv_load_policy=3&rel=0';
		$('#frame-trailer').attr('src', 'https://www.youtube.com/embed/' + video + options);
	});
	$('#modal-trailer').on('hide.bs.modal', function (event) {
		$('#frame-trailer').attr('src', null);
	});

	/*	Avatar preview in user profile
		It uses ajax call to get the correct image and load it
		on the image preview div.
	*/
	$('#avatar-select').on('change', function () {
		var imgFile = $(this).val();
		$.ajax({
			type: 'POST',
			url: '/Profile/GetAvatar',
			data: { id: imgFile },
			success: function (result) {
				$('#avatar-preview').html('<img class="img-fluid rounded" src="/images/' + result.file + '" alt="user avatar" />');
			}
		});
	});

	/*	Shows table filter
		It uses jQuery load function to refresh the shows needed to be displayed
		based on the user preference and selections.
	*/
	$('#order-filter').click(function () {
		var movieID = $('#order-sel-movie').val();
		var dateID = $('#order-sel-date').val();
		$('#order-shows-list').load('Order/ShowsTable?movieID=' + movieID + '&date=' + dateID);
	});

	$(document).on('click', '.btn-date', function () {
		var movieID = $('#order-sel-movie').val();
		var dateID = $(this).data("date");
		$('#order-shows-list').load('Order/ShowsTable?movieID=' + movieID + '&date=' + dateID);
	});
});