<script>
    $(document).ready(function () {
        $('.nav-link').on('click', function () {
            // Remove 'active' class from all navigation items
            $('.nav-link').removeClass('active');

            // Add 'active' class to the clicked navigation item
            $(this).addClass('active');
        });
});
</script>