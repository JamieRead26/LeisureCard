﻿var offersExperienceController = angular.module('offersExperienceController', []);

offersExperienceController.controller('offersExperienceController', function ($scope, $http, config, slideshow) {

    $scope.global.bodyclass = 'offer-experience';
    $scope.global.slideshow = slideshow.offerexperience;

    var url = config.apiUrl + '/RedLetter/GetRandomSpecialOffers/6';
    $http.get(url).then(function(r){
        $scope.special_offers = r.data.$values;
    });

    $scope.cateogies = [
        {
            name: 'Driving',
            description: 'Experience driving!',
            link: '/Experience/Driving?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Famous Circuits',
            description: '',
            link: '/Experience/Famous-Circuits?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Couples',
            description: '',
            link: '/Experience/Couples?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Gourmet',
            description: '',
            link: '/Experience/Gourmet?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Short Breaks',
            description: '',
            link: '/Experience/Short-Breaks?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Pampering',
            description: '',
            link: '/Experience/Pampering?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'London',
            description: '',
            link: '/Experience/London?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Flying',
            description: '',
            link: '/Experience/Flying?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Hot Air Ballooning',
            description: '',
            link: '/Experience/Hot-Air-Ballooning?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Adventure',
            description: '',
            link: '/Experience/Adventure?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Sports',
            description: '',
            link: '/Experience/Sports?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Culture',
            description: '',
            link: '/Experience/Culture?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Animals',
            description: '',
            link: '/Experience/Animals?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Hobbies',
            description: '',
            link: '/Experience/Hobbies?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Water',
            description: '',
            link: '/Experience/Water?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Kids',
            description: '',
            link: '/Experience/Kids?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Celebrities & Heroes',
            description: '',
            link: '/ideas/celebrities-and-heroes?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Traditional Gifts',
            description: '',
            link: '/Experience/Other-Gifts?source=rld&affid=212&dealid=28582'
        }
    ];
});