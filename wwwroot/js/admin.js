$(function () {
	/*	Image preview in admin CRUD pages
		It uses ajax call to get the correct image and load it
		on the image preview div.
	*/
	$('#admin-image-select').on('change', function () {
		var imgFile = $(this).val();
		$.ajax({
			type: 'POST',
			url: '/Admin/Images/GetImage',
			data: { id: imgFile },
			success: function (result) {
				$('#admin-image-preview').html('<img src="/images/' + result.file + '" />');
			}
		});
	});
});
