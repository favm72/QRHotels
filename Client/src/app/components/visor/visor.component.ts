import { Component, OnInit } from "@angular/core"
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser"
import { Router } from "@angular/router"
import { baseUrl } from "src/constants"

@Component({
	selector: "app-visor",
	templateUrl: "./visor.component.html",
	styleUrls: ["./visor.component.css"],
})
export class VisorComponent implements OnInit {
	safeURL: SafeResourceUrl

	constructor(private router: Router, private sanitizer: DomSanitizer) {
		let url = ""
		switch (this.router.url) {
			case "/master/cocinaacasa":
				url = baseUrl + "pdf/experiencias.pdf"
				break
			case "/master/experiencia":
				try {
					const userInfo = JSON.parse(localStorage.getItem("qrh"))
					const client = userInfo.client
					const reseCode = client.split("-")[1]
					const reseYear = client.split("-")[0]
					const reseId = `${reseCode}%2F${reseYear}`
					const guestCode = 1
					url = `https://surveys.myhotel.cl/survey-public/surveys-open/gRODgS56R6O6h8eBymV7fg?lang=es&reservation_id=${reseId}&guest_code=${guestCode}`
				} catch (error) {
					console.error("error de url encuesta")
				}
				break
			case "/master/embed":
				try {
					url = router.getCurrentNavigation().extras.state.url
				} catch (error) {
					console.error("No se envió una URL")
				}
				break
			default:
				break
		}
		this.safeURL = this.sanitizer.bypassSecurityTrustResourceUrl(url)
	}

	ngOnInit(): void {}
}
