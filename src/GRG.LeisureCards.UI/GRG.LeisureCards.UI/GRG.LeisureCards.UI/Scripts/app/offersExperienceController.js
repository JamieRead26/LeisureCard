var offersExperienceController = angular.module('offersExperienceController', []);

offersExperienceController.factory('RedLetterGetSpecialOffers', function ($resource, config) {
    return $resource(config.apiUrl + '/RedLetter/GetRandomSpecialOffers/:count');
});

offersExperienceController.controller('offersExperienceController', function ($scope, RedLetterGetSpecialOffers) {
    $scope.global.slideshow = [
        {
            img:  'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        }
    ];

    RedLetterGetSpecialOffers.get({ count: 6 }, function(data) {
        $scope.special_offers = data.$values;
    });

    $scope.cateogies = [
        {
            name: 'Driving',
            description: 'Experience driving!',
            link: '/Home?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Famous Circuits',
            description: '',
            link: '/Experience/Famous-Circuits?source=rld&affid=212&dealid=28582'
        },
        {
            name: 'Couples',
            description: '',
            link: '/Experience/Gourmet?source=rld&affid=212&dealid=28582'
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
            name: 'Other',
            description: '',
            link: '/Experience/Other-Gifts?source=rld&affid=212&dealid=28582'
        }
    ];
});