import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { InfoPagesService } from "src/app/services/info-pages.service"

@Component({
	selector: "app-informative",
	templateUrl: "./informative.component.html",
	styleUrls: ["./informative.component.css"],
})
export class InformativeComponent implements OnInit {
	title: string = ""
	detail = []
	constructor(private infoPage: InfoPagesService, private router: Router) {}

	async ngOnInit() {
		if (this.infoPage.currentPageId) this.detail = await this.infoPage.listDetail(+this.infoPage.currentPageId)
		else this.router.navigate(["master/main"])
	}

	decode(str) {
		return decodeURIComponent(atob(str))
	}

	openLink(item) {
		window.open(item.linkUrl, "_blank")
	}

	openMap(item) {
		window.open(item.mapUrl, "_blank")
	}
}
