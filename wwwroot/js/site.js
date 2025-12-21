// ===== Toast Notification System =====
function showToast(message, type = 'info') {
    const toastContainer = document.getElementById('toastContainer');
    if (!toastContainer) return;

    const toastId = 'toast-' + Date.now();
    const iconMap = {
        success: 'check-circle-fill',
        error: 'exclamation-triangle-fill',
        warning: 'exclamation-triangle-fill',
        info: 'info-circle-fill'
    };

    const bgMap = {
        success: 'bg-success',
        error: 'bg-danger',
        warning: 'bg-warning',
        info: 'bg-info'
    };

    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-white ${bgMap[type]} border-0" role="alert">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="bi bi-${iconMap[type]} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;

    toastContainer.insertAdjacentHTML('beforeend', toastHtml);

    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement, { delay: 3000 });
    toast.show();

    toastElement.addEventListener('hidden.bs.toast', () => {
        toastElement.remove();
    });
}

// ===== Favorite Toggle =====
async function toggleFavorite(recipeId) {
    try {
        // Butonu bul - farklı selector'lar dene
        let button = document.querySelector(`[data-recipe-id="${recipeId}"]`);
        if (!button) {
            button = document.querySelector(`button[onclick*="${recipeId}"]`);
        }
        
        if (!button) {
            console.error('Button not found for recipe:', recipeId);
            showToast('Bir hata oluştu', 'error');
            return;
        }

        const icon = button.querySelector('i');
        const isFavorite = icon.classList.contains('bi-heart-fill');

        // Route parametresi kullan (query string değil)
        const url = isFavorite
            ? `/Recipe/RemoveFromFavorites/${recipeId}`
            : `/Recipe/AddToFavorites/${recipeId}`;

        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        const result = await response.json();

        if (result.success) {
            // UI'ı güncelle
            if (isFavorite) {
                icon.classList.remove('bi-heart-fill');
                icon.classList.add('bi-heart');
                button.classList.remove('active');
                showToast(result.message || 'Favorilerden çıkarıldı', 'info');
            } else {
                icon.classList.remove('bi-heart');
                icon.classList.add('bi-heart-fill');
                button.classList.add('active');
                showToast(result.message || 'Favorilere eklendi', 'success');
            }
        } else {
            showToast(result.message || 'Bir hata oluştu', 'error');
            if (result.message && result.message.includes('giriş')) {
                setTimeout(() => {
                    window.location.href = '/Auth/Login';
                }, 1500);
            }
        }
    } catch (error) {
        console.error('Error toggling favorite:', error);
        showToast('Bir hata oluştu. Lütfen tekrar deneyin.', 'error');
    }
}

// ===== Form Validation =====
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return false;

    const inputs = form.querySelectorAll('input[required], textarea[required], select[required]');
    let isValid = true;

    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    return isValid;
}

// ===== Loading Overlay =====
function showLoading() {
    let overlay = document.querySelector('.loading-overlay');
    if (!overlay) {
        overlay = document.createElement('div');
        overlay.className = 'loading-overlay';
        overlay.innerHTML = '<div class="spinner-border text-light spinner-border-lg" role="status"></div>';
        document.body.appendChild(overlay);
    }
    overlay.classList.add('active');
}

function hideLoading() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) {
        overlay.classList.remove('active');
    }
}

// ===== Auto-dismiss alerts =====
document.addEventListener('DOMContentLoaded', function() {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});
