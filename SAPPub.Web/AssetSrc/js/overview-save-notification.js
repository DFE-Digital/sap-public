(function () {
    function init() {
        const banner = document.querySelector('[id$="-add-success"], [id$="-remove-success]');

        if (!banner) {
            return;
        }

        banner.scrollIntoView({ behavior: 'smooth', block: 'start' });

        banner.setAttribute('tabindex', '-1');
        banner.focus({ preventScroll: true });
    }

    document.addEventListener('DOMContentLoaded', init);
})();