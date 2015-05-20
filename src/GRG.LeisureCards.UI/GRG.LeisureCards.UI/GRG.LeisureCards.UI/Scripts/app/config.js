app.constant('config', {
    googleAPiKey: 'AIzaSyAvl80GCl1HRw1H_FmBIGlLk2VPcV5R5Cg',
    tenant: 'base',
    apiUrl: '//localhost:1623' //-http://grg.leisurecards.api.shiftkey.uk.com
});

app.service('slideshow', function (config) {

    var tenant = config.tenant;

    this.login = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: 'http://google.co.uk'
    },
    {
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: 'http://google.co.uk'
    }];

    this.offershome = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
    }];

    this.offer241 = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
    }];
    this.offer241details = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
     }];
    this.offer241claim = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
    }];

    this.offerexperience = [{
        img: '/Content/' + tenant + '/img/banner-1.jpg',
        link: ''
    }];

    this.offerdiscount = [];
    this.offerdiscountdetails = [];
    this.offerdiscountclaim = [];

});
