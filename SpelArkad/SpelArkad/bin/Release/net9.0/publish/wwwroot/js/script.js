document.addEventListener('DOMContentLoaded', function () {
    const categoryFilter = document.getElementById('categoryFilter');
    const gameCards = document.querySelectorAll('.game-card');

    // Modal element
    const modal = document.getElementById('gameModal');
    const closeBtn = document.querySelector('.close');
    const modalTitle = document.getElementById('modalTitle');
    const modalVideo = document.getElementById('modalVideo');
    const modalDescription = document.getElementById('modalDescription');

    // Filtrering av spel
    categoryFilter.addEventListener('change', function () {
        const selectedCategory = categoryFilter.value;

        gameCards.forEach(function (card) {
            const cardCategory = card.getAttribute('data-category');

            if (selectedCategory === 'all' || selectedCategory === cardCategory) {
                card.style.display = 'block'; // Visa kortet om det matchar den valda kategorin
            } else {
                card.style.display = 'none'; // Dölja kortet om det inte matchar
            }
        });
    });

    // Funktion för att konvertera YouTube-länk till en inbäddningsvänlig URL
    function getEmbedUrl(url) {
        if (url.includes("youtube.com/watch?v=")) {
            const videoId = new URL(url).searchParams.get("v"); // Hämta video-ID
            return `https://www.youtube.com/embed/${videoId}?autoplay=1`;
        } else if (url.includes("youtu.be/")) {
            const videoId = url.split("youtu.be/")[1].split("?")[0]; // Extrahera video-ID
            return `https://www.youtube.com/embed/${videoId}?autoplay=1`;
        }
        return url; // Om det inte är en YouTube-länk, returnera samma URL
    }

    // Funktion för att öppna modalen
    function openModal(trailerUrl, title, description) {
        // Konvertera YouTube-länk till embed-format om den är en vanlig delningslänk
        if (trailerUrl.includes("watch?v=")) {
            trailerUrl = trailerUrl.replace("watch?v=", "embed/");
        }

        // Sätt modalens innehåll
        document.getElementById('modalTitle').innerText = title;
        document.getElementById('modalDescription').innerText = description;
        document.getElementById('modalVideo').src = trailerUrl;

        // Visa modalen
        document.getElementById('gameModal').style.display = 'block';
    }

    // Stäng modalen och stoppa videon
    document.querySelector('.close').addEventListener('click', function () {
        document.getElementById('gameModal').style.display = 'none';
        document.getElementById('modalVideo').src = ''; // Stoppa videon
    });


    // Lyssna på "Se Trailer"-knapparna istället för hela kortet
    document.querySelectorAll('.game-card button').forEach(button => {
        button.addEventListener('click', function (event) {
            event.stopPropagation(); // Förhindra att klick på kortet triggas

            const card = event.target.closest('.game-card'); // Hitta närmaste kort
            const title = card.querySelector('h3').innerText;
            const description = card.querySelector('p').innerText;
            const videoSrc = card.getAttribute('data-video');

            openModal(videoSrc, title, description);
        });
    });

    // Stäng modalen när användaren klickar på stängknappen
    closeBtn.addEventListener('click', function () {
        modal.style.display = 'none';
        modalVideo.src = ''; // Stoppa videon när modal stängs
    });

    // Stäng modalen om användaren klickar utanför modalen
    window.addEventListener('click', function (event) {
        if (event.target === modal) {
            modal.style.display = 'none';
            modalVideo.src = ''; // Stoppa videon när modal stängs
        }
    });
});

$(document).ready(function () {
    // Hämta alla spel från API:et när sidan laddas
    $.get("https://localhost:5232/api/spel", function (data) {
        var gameGrid = $(".game-grid");
        gameGrid.empty(); // Töm grid innan nya kort läggs till

        // Gå igenom alla spel och lägg till varje spel som ett kort
        data.forEach(function (spel) {
            var gameCard = `
                <div class="game-card" data-category="${spel.Kategori}" data-video="${spel.TrailerUrl}">
                    <div class="badge">HOT 🔥</div>
                    <img src="${spel.BildUrl}" alt="${spel.Titel}">
                    <h3>${spel.Titel}</h3>
                    <p>${spel.Kategori}</p>
                    <button onclick="openModal('${spel.TrailerUrl}', '${spel.Titel}', 'Kategori: ${spel.Kategori}')">Se Trailer</button>
                </div>
            `;
            gameGrid.append(gameCard);
        });
    }).fail(function () {
        alert("Det gick inte att hämta spel från API.");
    });

    // Hantera formulärinlämning för att skapa ett nytt spel
    $("#createGame").submit(function (event) {
        event.preventDefault();

        // Hämta data från formuläret
        const titel = $("#titel").val();
        const kategori = $("#kategori").val();
        const beskrivning = $("#beskrivning").val();
        const bildUrl = $("#bildUrl").val();
        const trailerUrl = $("#trailerUrl").val();

        // Skapa ett objekt för det nya spelet
        const gameData = {
            Titel: titel,
            Kategori: kategori,
            Beskrivning: beskrivning,
            BildUrl: bildUrl,
            TrailerUrl: trailerUrl
        };

        // Skicka en POST-begäran till API:et
        $.ajax({
            url: "https://localhost:5232/api/spel",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(gameData),
            success: function (data) {
                alert("Spelet har skapats!");
                // Uppdatera spelgriden på sidan genom att hämta de senaste spelen
                $.get("https://localhost:5232/api/spel", function (data) {
                    var gameGrid = $(".game-grid");
                    gameGrid.empty();
                    data.forEach(function (spel) {
                        var gameCard = `
                            <div class="game-card" data-category="${spel.Kategori}" data-video="${spel.TrailerUrl}">
                                <div class="badge">HOT 🔥</div>
                                <img src="${spel.BildUrl}" alt="${spel.Titel}">
                                <h3>${spel.Titel}</h3>
                                <p>${spel.Kategori}</p>
                                <button onclick="openModal('${spel.TrailerUrl}', '${spel.Titel}', 'Kategori: ${spel.Kategori}')">Se Trailer</button>
                            </div>
                        `;
                        gameGrid.append(gameCard);
                    });
                });

                // Stäng modalen eller omdirigera till index-sidan
                window.location.href = "/Home/Index";
            },
            error: function () {
                alert("Det gick inte att skapa spelet. Försök igen.");
            }
        });
    });
});
