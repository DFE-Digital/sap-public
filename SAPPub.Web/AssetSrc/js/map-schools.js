(function () {
    const host = document.getElementById("map");
    if (!host) return;

    const fixedZoom = parseInt(host.dataset.fixedZoom || "14", 10);

    const lat = parseFloat(host.dataset.lat);
    const lon = parseFloat(host.dataset.lon);

    if (!Number.isFinite(lat) || !Number.isFinite(lon)) {
        console.error("Map lat/lon missing or invalid. Check data-lat/data-lon on #map.", { lat, lon });
        const loading = host.querySelector(".map-loading");
        if (loading) loading.textContent = "Location not available for this school.";
        return;
    }

    // Optional: if you want the name in the popup, put it on the div too (data-name)
    const schoolName = host.dataset.name || "School location";

    // ============================================================
    // ICON
    // ============================================================
    const schoolIcon = L.icon({
        iconUrl: "/assets/images/marker-school-target.svg",
        iconSize: [36, 54],
        iconAnchor: [18, 52],
        popupAnchor: [0, -44],
    });

    // ============================================================
    // ZOOM HINT
    // ============================================================
    function showZoomHint(mapContainer) {
        let hint = mapContainer.querySelector(".map-zoom-hint");
        if (!hint) {
            hint = document.createElement("div");
            hint.className = "map-zoom-hint";
            hint.textContent = "Use Ctrl + scroll to zoom the map";
            mapContainer.appendChild(hint);
        }
        hint.classList.add("visible");
        clearTimeout(hint._hideTimer);
        hint._hideTimer = setTimeout(() => hint.classList.remove("visible"), 3000);
    }

    // ============================================================
    // ACCESSIBILITY (TAB + ENTER/SPACE)
    // ============================================================
    function a11yMarker(marker, label) {
        marker.on("add", () => {
            const el = marker.getElement?.() || marker._icon;
            if (!el) return;

            el.setAttribute("role", "button");
            el.setAttribute("tabindex", "0");
            el.setAttribute("aria-label", label);
            el.classList.add("marker-focus");

            el.addEventListener("keydown", (e) => {
                if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    marker.openPopup();
                }
            });
        });
        return marker;
    }

    // ============================================================
    // MAIN
    // ============================================================
    const schoolLL = L.latLng(lat, lon);

    const loading = host.querySelector(".map-loading");
    if (loading) loading.remove();

    const map = L.map(host, { scrollWheelZoom: true }).setView(schoolLL, fixedZoom);

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        maxZoom: 19,
        attribution: "© OpenStreetMap contributors",
    }).addTo(map);

    setTimeout(() => map.invalidateSize(), 0);

    // Scroll behaviour (Ctrl+wheel)
    let zoomHintShown = false;
    map.getContainer().addEventListener(
        "wheel",
        (e) => {
            if (!e.ctrlKey && !e.metaKey) {
                e.preventDefault();
                if (!zoomHintShown) showZoomHint(host);
                zoomHintShown = true;
            }
        },
        { passive: false }
    );

    map.on("mouseover", () => map.scrollWheelZoom.enable());
    map.on("mouseout", () => map.scrollWheelZoom.disable());

    const marker = L.marker(schoolLL, { icon: schoolIcon })
        .bindPopup(`<strong>${schoolName}</strong>`)
        .addTo(map);

    a11yMarker(marker, `${schoolName}, map marker`);
})();