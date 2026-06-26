(function () {
    const host = document.getElementById("map");
    if (!host) return;

    const fixedZoom = parseInt(host.dataset.fixedZoom || "14", 10);

    let points = null;

    if (host.dataset.points) {
        try {
            const parsed = JSON.parse(host.dataset.points);
            if (Array.isArray(parsed)) {
                points = parsed.map(p => ({
                    lat: parseFloat(p.lat),
                    lon: parseFloat(p.lng),
                    name: p.name || "School location"
                }));
            }
            else {
                console.error("Map lat/lon missing or invalid. Check data-lat/data-lon on #map.", { lat, lon });
                const loading = host.querySelector(".map-loading");
                if (loading) loading.textContent = "Location not available for this school.";
                return; cons
            }
        }
        catch (e) {
            console.error("Failed to parse data-points JSON", { lat, lon });
        }
    }

    if (!points || points.length === 0) {
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
        points = [{ lat, lon, name: schoolName }];
    }

    // ============================================================
    // ICONS
    // ============================================================
    const iconSize = [36, 54];
    const iconAnchor = [18, 52];
    const popupAnchor = [0, -44];

    const MAX_ICONS = 6;
    const icons = [];
    for (let a = 0; a < MAX_ICONS; a++) {
        const iconUrl = `/assets/images/marker-school-${a+1}.svg`;
        icons.push(L.icon({ iconUrl, iconSize, iconAnchor, popupAnchor }));
    }

    function getIconForIndex(index) {
        const idx = Math.min(index, icons.length - 1);
        return icons[idx];
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
    const loading = host.querySelector(".map-loading");
    if (loading) loading.remove();

    const first = points[0];
    const initialLL = L.latLng(first.lat, first.lon);

    const map = L.map(host, {
        scrollWheelZoom: true,
        gestureHandling: true
    }).setView(initialLL, fixedZoom);

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        maxZoom: 19,
        attribution: "© OpenStreetMap contributors",
    }).addTo(map);

    setTimeout(() => map.invalidateSize(), 0);

    const latLngs = [];
    points.forEach((p, i) => {
        const ll = L.latLng(p.lat, p.lon);
        latLngs.push(ll);

        const icon = getIconForIndex(i);
        const marker = L.marker(ll, { icon })
            .bindPopup(`<strong>${p.name}</strong>`)
            .addTo(map);

        a11yMarker(marker, `${p.name}, map marker`);
    });

    if (latLngs.length > 1) {
        const bounds = L.latLngBounds(latLngs);
        setTimeout(() => {
            map.invalidateSize();
            map.fitBounds(bounds, { padding: [40, 40] });
        }, 200);
    }


    // Map should be hidden by default. If javascript is switched on, this will kick in and unhide the map
    function showMap() {
        var mapContainers = document.getElementsByClassName("map-container");
        if (mapContainers) {
            for (let i = 0; i < mapContainers.length; i++) {
                mapContainers[i].classList.remove("govuk-visually-hidden");
            }
        }
    }

    document.addEventListener('DOMContentLoaded', showMap);

})();