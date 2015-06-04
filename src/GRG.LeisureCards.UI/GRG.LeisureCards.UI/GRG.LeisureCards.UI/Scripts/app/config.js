app.constant('config', {
    googleAPiKey: 'AIzaSyAvl80GCl1HRw1H_FmBIGlLk2VPcV5R5Cg',
    tenant: 'GRG',
    apiUrl: '//localhost:1623' //-http://grg.leisurecards.api.shiftkey.uk.com
});

app.service('slideshow', function (config) {

    var tenant = config.tenant;

    this.login = [{       
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: 'http://google.co.uk'
    }];
    this.offershome = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
    },
    {
        img: '/Content/' + tenant + '/img/banner-2.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-3.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-4.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-5.jpg',
        link: 'http://google.co.uk'
    }];

    this.offer241 = [{
        img: '/Content/' + tenant + '/img/banner-6.jpg',
        link: ''
    },
    {
        img: '/Content/' + tenant + '/img/banner-7.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-8.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-9.jpg',
        link: 'http://google.co.uk'
    }];

    this.offer241details = [{
        img: '/Content/' + tenant + '/img/banner-6.jpg',
        link: ''
    }];

    this.offer241claim = [{
        img: '/Content/' + tenant + '/img/banner-6.jpg',
        link: ''
    }];

    this.offerexperience = [{
        img: '/Content/' + tenant + '/img/banner-16.jpg',
        link: ''
    },
     {
         img: '/Content/' + tenant + '/img/banner-17.jpg',
         link: 'http://google.co.uk'
     },
     {
         img: '/Content/' + tenant + '/img/banner-18.jpg',
         link: 'http://google.co.uk'
     },
     {
         img: '/Content/' + tenant + '/img/banner-19.jpg',
         link: 'http://google.co.uk'
     },
     {
         img: '/Content/' + tenant + '/img/banner-20.jpg',
         link: 'http://google.co.uk'
     },
     {
         img: '/Content/' + tenant + '/img/banner-21.jpg',
         link: 'http://google.co.uk'
     }];

    this.terms = [{
        img: '/Content/' + tenant + '/img/banner-8.jpg',
        link: ''
    }];

    this.offerdiscount = [{
        img: '/Content/' + tenant + '/img/banner-6.jpg',
        link: 'http://google.co.uk'
    }];
    this.offerdiscountcottage = [{
        img: '/Content/' + tenant + '/img/banner-11.jpg',
        link: 'http://google.co.uk'
    }];
    this.offerdiscounthoseasons = [{
        img: '/Content/' + tenant + '/img/banner-11.jpg',
        link: 'http://google.co.uk'
    }];
    this.offerdiscountclaim = [];

});
