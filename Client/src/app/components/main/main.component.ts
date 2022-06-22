import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { AnalyticsService } from "src/app/services/analytics.service"
import { CartService } from "src/app/services/cart.service"
import { HomeMenuService, HomeMenuType } from "src/app/services/home-menu.service"
import { InfoPagesService } from "src/app/services/info-pages.service"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-main",
	templateUrl: "./main.component.html",
	styleUrls: ["./main.component.css"],
})
export class MainComponent implements OnInit {
	constructor(
		private router: Router,
		private analytics: AnalyticsService,
		private info: InfoService,
		private cart: CartService,
		private notif: NotificationService,
		private home: HomeMenuService,
		private infoPage: InfoPagesService
	) {}

	pictures = []
	sliderTitle = ""
	hotelType = 0
	showRestaurants = false
	imageRestaurants = null
	menu = []

	async ngOnInit(): Promise<void> {
		this.menu = await this.home.list()

		this.pictures = await this.info.loadSlider()
		if (this.pictures.length > 0) this.sliderTitle = this.pictures[0].title
		this.analytics.log(`home`)

		const hotelInfo = await this.info.loadHotelInfo()

		this.hotelType = hotelInfo.hotelType
		this.showRestaurants = hotelInfo.showRestaurants
		this.imageRestaurants = hotelInfo.imageRestaurants
	}

	visualize(url) {
		this.analytics.log(`Banner ` + this.sliderTitle)
		this.router.navigate(["/master/embed"], { state: { url: url } })
	}

	async goToCart(provider): Promise<void> {
		this.analytics.log(`cart/${provider}`)
		this.cart.provider = provider
		this.cart.viewOnly = true
		let verify = await this.cart.enabled(provider)
		if (verify.data) this.router.navigate(["/cart"])
		else this.notif.info(verify.message)
	}

	async goToAmmenities(): Promise<void> {
		this.analytics.log(`cart/15`)
		this.cart.provider = 15
		let verify = await this.cart.enabled(15)
		if (verify.data) this.router.navigate(["/master/ammenities"])
		else this.notif.info(verify.message)
	}

	async goToLink(item) {
		switch (item.linkUrl) {
			case HomeMenuType.restaurants.id:
				this.analytics.log(`Alimentación`)
				this.router.navigate(["/master/digital"])
				break
			case HomeMenuType.preCheck.id:
				this.analytics.log(`Pre Check`)
				this.router.navigate(["/intake"])
				break
			case HomeMenuType.lateCheck.id:
				await this.analytics.log("latecheckout")
				let verify = await this.cart.enabled(3)
				if (verify.data) this.router.navigate(["/master/latecheckout"])
				else this.notif.info(verify.message)
				break
			case HomeMenuType.services.id:
				this.analytics.log(`Services`)
				this.router.navigate(["/master/services"])
				break
			case HomeMenuType.experience.id:
				this.analytics.log(`Experiencia`)
				this.router.navigate(["/master/experiencia"])
				break
			case HomeMenuType.cleaning.id:
				this.analytics.log(`Limpieza`)
				this.router.navigate(["/master/limpieza"])
				break
			case HomeMenuType.store.id:
				this.goToCart(1)
				break
			case HomeMenuType.sama.id:
				this.goToCart(7)
				break
			case HomeMenuType.ammenities.id:
				this.goToAmmenities()
				break
			case HomeMenuType.link.id:
				console.log(item.link)
				window.open(item.link, "_blank")
				break
			case HomeMenuType.informative.id:
				this.infoPage.currentPageId = item.idInfoPage
				this.router.navigate(["/master/informative"])
				break
			default:
				break
		}
	}

	setTitle(index) {
		this.sliderTitle = this.pictures[index].title
	}
}
