// Write your JavaScript code.


document.getElementById('expand1').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon1'), 'content1');
});

document.getElementById('expand2').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon2'), 'content2');
});

document.getElementById('expand3').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon3'), 'content3');
});

document.getElementById('expand4').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon4'), 'content4');
});

document.getElementById('expand5').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon5'), 'content5');
});

document.getElementById('expand6').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon6'), 'content6');
});

document.getElementById('expand7').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon7'), 'content7');
});

document.getElementById('expand8').addEventListener('click', function () {
    toggleIcon(document.getElementById('toggleIcon8'), 'content8');
});


function toggleIcon(icon, content) {
    toggleContainer(document.getElementById(content));
    if (icon.classList.contains('fa-plus')) {
        icon.classList.remove('fa-plus');
        icon.classList.add('fa-minus');
    } else {
        icon.classList.remove('fa-minus');
        icon.classList.add('fa-plus');
    }
}

function toggleContainer(content) {
    if (content.style.maxHeight && content.style.maxHeight != '0px') {
        // Collapse the container
        content.style.maxHeight = '0px';
    } else {
        // Expand the container based on its scroll height
        content.style.maxHeight = content.scrollHeight + "px";
    }
}


function loadData() {
    const buttonText = document.getElementById('buttonText');
    const loadingIcon = document.getElementById('loadingIcon');

    buttonText.style.display = 'none';
    loadingIcon.style.display = 'inline-block';

    setTimeout(() => {
        loadingIcon.style.display = 'none';
        buttonText.style.display = 'inline-block';
    }, 3000);
}

function showIcon(button) {
    var icon = button.querySelector('i');
    if (icon) {
        icon.style.visibility = 'visible';
    }
}