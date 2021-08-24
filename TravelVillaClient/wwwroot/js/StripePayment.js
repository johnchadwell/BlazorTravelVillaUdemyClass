redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51JPzIdLWwqmEiNRdk2hCzHQu6xQ8ybohaj0olVRRIvi07nnkJPBMgIFzy7jHE2RLGJBWykTC7L6zO5pc1QV3IcPY00ywDh6iMm');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};