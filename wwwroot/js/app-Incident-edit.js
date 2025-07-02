/**
 * App Invoice - Edit
 */

'use strict';

(function () {
  const //invoiceItemPriceList = document.querySelectorAll('.invoice-item-price'),
    // invoiceItemQtyList = document.querySelectorAll('.invoice-item-qty'),
    date = new Date(),
    incidentDate = document.querySelector('.incident-date'),
    closedDate = document.querySelector('.closed-date'),
    folowupDate = document.querySelector('.folowup-date');
  // Price
  // if (invoiceItemPriceList) {
  //   invoiceItemPriceList.forEach(function (invoiceItemPrice) {
  //     new Cleave(invoiceItemPrice, {
  //       delimiter: '',
  //       numeral: true
  //     });
  //   });
  // }

  // // Qty
  // if (invoiceItemQtyList) {
  //   invoiceItemQtyList.forEach(function (invoiceItemQty) {
  //     new Cleave(invoiceItemQty, {
  //       delimiter: '',
  //       numeral: true
  //     });
  //   });
  // }

  // Datepicker
  if (incidentDate) {
    incidentDate.flatpickr({
      monthSelectorType: 'static',
      defaultDate: date,
      dateFormat: 'm/d/Y'
    });
  }
  if (closedDate) {
    closedDate.flatpickr({
      monthSelectorType: 'static',
      defaultDate: new Date(date.getFullYear(), date.getMonth(), date.getDate() + 5),
      dateFormat: 'm/d/Y'
    });
  }
  if (folowupDate) {
    folowupDate.flatpickr({
      monthSelectorType: 'static',
      defaultDate: new Date(date.getFullYear(), date.getMonth(), date.getDate()),
      dateFormat: 'm/d/Y'
    });
    console.log(folowupDate);
  }
})();

// repeater (jquery)
$(function () {
  console.log('repeater');
  // Ensure the repeater plugin is loaded before this script
  // if (typeof $.fn.repeater === 'undefined') {
  //   console.error('jQuery Repeater plugin is not loaded. Please include jquery.repeater.js before this script.');
  //   return;
  // }

  var $repeaterContainer = $('.repeater-container'), // Use the correct container for the repeater
    incidentDetails = {
      Administration: 'Administration of the project',
      Purchase: 'Purchasing different goods for baking the cake',
      workingHours: 'Details of the working hours ...',
      'App Customization': 'Customization & Bug Fixes.',
      'ABC Template': 'Bootstrap 4 admin template.',
      'App Development': 'Native App Development.'
    };

  // Repeater init
  if ($repeaterContainer.length) {
    $repeaterContainer.repeater({
      show: function () {
        $(this).slideDown();
      },
      hide: function (deleteElement) {
        $(this).slideUp(deleteElement);
      }
    });
  }
});
